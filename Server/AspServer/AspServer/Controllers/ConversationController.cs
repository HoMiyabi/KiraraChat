using AspServer.Models;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace AspServer.Controllers;

[ApiController]
public class ConversationController : Controller
{
    //[HttpPost("get_friend_and_unread_count")]
    //public async Task<IActionResult> GetFriendAndUnreadCount([FromBody] CS<SessionID> csSessionID)
    //{
    //    if (!Identity.Authenticate(csSessionID.data.sessionID, out int userID))
    //    {
    //        return Ok(new SC<FriendAndUnreadCount>
    //        {
    //            code = 0,
    //            message = "未登录"
    //        });
    //    }

    //    List<FriendAndUnreadCount> friendUserInfos =
    //        await Program.db.Queryable<UserTable, FriendRelationshipDO, UnreadCountDO>
    //        ((user, relationship, unreadCount)
    //        => user.id == relationship.user_id2 && unreadCount.sender_id == user.id)
    //        .Where((user, relationship) => relationship.user_id1 == userID)
    //        .OrderBy(user => user.id)
    //        .Select((user, _, unreadCount) => new FriendAndUnreadCount
    //        {
    //            userInfo = new ClientUserInfo
    //            {
    //                id = user.id,
    //                username = user.username,
    //                signature = user.signature,
    //                profilePhotoName = user.profile_photo_name
    //            },
    //            unreadCount = unreadCount.count
    //        })
    //        .ToListAsync();

    //    return Ok(new SC<List<FriendAndUnreadCount>>
    //    {
    //        code = 0,
    //        data = null,
    //    });
    //}

    [HttpPost("getconversation")]
    public async Task<IActionResult> Index([FromBody] GetConversationCS getConversationCS)
    {
        if (!Identity.Authenticate(getConversationCS.sessionID, out int userID))
        {
            return Ok(new GetConversationSC
            {
                success = false,
                info = "未登录"
            });
        }

        int friendID = getConversationCS.friendID;

        bool bFriend = await Program.db.Queryable<FriendRelationshipDO>()
            .WhereClassByPrimaryKey(new FriendRelationshipDO
            {
                user_id1 = userID,
                user_id2 = friendID
            })
            .AnyAsync();

        if (!bFriend)
        {
            return Ok(new GetConversationSC
            {
                success = false,
                info = "不是好友"
            });
        }

        List<Message> messages = 
            await Program.db.Queryable<ConversationTable>()
            .Where(conversation => 
            (conversation.sender_id == userID && conversation.receiver_id == friendID) ||
            (conversation.sender_id == friendID && conversation.receiver_id == userID))
            .OrderBy(conversation => conversation.id)
            .Select(conversation => new Message
            {
                senderID = conversation.sender_id,
                receiverID = conversation.receiver_id,
                messageKind = (MessageKind)conversation.kind,
                dateTime = conversation.date_time,
                content = conversation.content
            })
            .ToListAsync();

        return Ok(new GetConversationSC
        {
            success = true,
            messages = messages
        });
    }
}