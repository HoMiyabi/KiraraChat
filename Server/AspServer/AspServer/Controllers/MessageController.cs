using AspServer.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Threading.Channels;

namespace AspServer.Controllers;

[ApiController]
public class MessageController : Controller
{
    public static ConcurrentDictionary<(int senderID, int receiverID), (Channel<Message>, CancellationTokenSource)> dict = 
        new();

    [HttpPost("sendmessage")]
    public async Task<IActionResult> SendMessage([FromBody] SendMessageCS sendMessageCS)
    {
        if (!Identity.Authenticate(sendMessageCS.sessionID, out int senderID))
        {
            return Ok(new SendMessageSC
            {
                success = false,
                info = "未登录"
            });
        }

        if (string.IsNullOrEmpty(sendMessageCS.content))
        {
            return Ok(new SendMessageSC
            {
                success = false,
                info = "content不得为null或empty"
            });
        }

        int receiverID = sendMessageCS.receiverID;

        bool bFriend = await Program.db.Queryable<FriendRelationshipDO>()
            .WhereClassByPrimaryKey(new FriendRelationshipDO
            {
                user_id1 = senderID,
                user_id2 = receiverID
            })
            .AnyAsync();

        if (!bFriend)
        {
            return Ok(new SendMessageSC
            {
                success = false,
                info = "不是好友"
            });
        }

        DateTime time = DateTime.Now;

        if (dict.TryGetValue((senderID, receiverID), out var value))
        {
            var (channel, _) = value;
            await channel.Writer.WriteAsync(new Message
            {
                senderID = senderID,
                receiverID = receiverID,
                messageKind = sendMessageCS.kind,
                dateTime = time,
                content = sendMessageCS.content
            });
        }

        await Program.db.Insertable(new ConversationTable
        {
            sender_id = senderID,
            receiver_id = receiverID,
            kind = (int)sendMessageCS.kind,
            date_time = time,
            content = sendMessageCS.content
        }).ExecuteCommandAsync();

        return Ok(new SendMessageSC
        {
            success = true
        });
    }

    [HttpPost("pollmessage")]
    public async Task<IActionResult> PollMessage([FromBody] PollMessageCS pollMessageCS)
    {
        if (!Identity.Authenticate(pollMessageCS.sessionID, out int receiverID))
        {
            return Ok(new SendMessageSC
            {
                success = false,
                info = "未登录"
            });
        }

        int senderID = pollMessageCS.senderID;

        // 这里报过 System.InvalidOperationException: This MySqlConnection is already in use.
        bool bFriend = await Program.db.CopyNew().Queryable<FriendRelationshipDO>()
            .WhereClassByPrimaryKey(new FriendRelationshipDO
            {
                user_id1 = senderID,
                user_id2 = receiverID
            })
            .AnyAsync(); 

        if (!bFriend)
        {
            return Ok(new SendMessageSC
            {
                success = false,
                info = "不是好友"
            });
        }
        
        if (dict.TryRemove((senderID, receiverID), out var value))
        {
            var (_, beforeCts) = value;
            Console.WriteLine($"sender {senderID}, receiver {receiverID}, 取消之前的CTS");
            beforeCts.Cancel();
        }

        var timeout = TimeSpan.FromSeconds(30);
        var cts = new CancellationTokenSource(timeout);

        var channel = Channel.CreateUnbounded<Message>();
        dict.TryAdd((senderID, receiverID), (channel, cts));

        try
        {
            Console.WriteLine($"sender {senderID}, receiver {receiverID}, 消息Poll中");
            Message message = await channel.Reader.ReadAsync(cts.Token);
            dict.TryRemove((senderID, receiverID), out var _);
            Console.WriteLine($"sender {senderID}, receiver {receiverID}, Poll成功");
            return Ok(new PollMessageSC
            {
                success = true,
                timeout = false,
                data = message
            });
        }
        catch (OperationCanceledException e)
        {
            dict.TryRemove((senderID, receiverID), out var _);
            Console.WriteLine($"sender {senderID}, receiver {receiverID}, Poll被取消, e {e.Message}");
            return Ok(new PollMessageSC
            {
                success = true,
                timeout = true
            });
        }
    }
}
