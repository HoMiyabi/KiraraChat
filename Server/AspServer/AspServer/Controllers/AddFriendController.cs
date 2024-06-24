using AspServer.Models;
using Microsoft.AspNetCore.Mvc;

namespace AspServer.Controllers;

[ApiController]
public class AddFriendController : Controller
{
    [HttpPost("addfriend")]
    public async Task<IActionResult> Index([FromBody] AddFriendCS addFriendC2S)
    {
        if (!Identity.Authenticate(addFriendC2S.sessionID, out int selfID))
        {
            return Ok(new AddFriendSC
            {
                success = false,
                info = "未登录"
            });
        }

        int otherID = addFriendC2S.otherID;

        if (selfID == otherID)
        {
            return Ok(new AddFriendSC
            {
                success = false,
                info = "不能添加自己为好友"
            });
        }

        // TODO)) 找不到会不会返回null？
        UserTable otherData = await Program.db.Queryable<UserTable>().InSingleAsync(otherID);
        if (otherData == null)
        {
            return Ok(new AddFriendSC
            {
                success = false,
                info = "用户不存在"
            });
        }

        // 判断已经是好友
        bool bAlreadyFriend = await Program.db.Queryable<FriendRelationshipDO>()
            .WhereClassByPrimaryKey(new FriendRelationshipDO
            {
                user_id1 = selfID,
                user_id2 = otherID
            })
            .AnyAsync();

        if (bAlreadyFriend)
        {
            return Ok(new AddFriendSC
            {
                success = false,
                info = "已经是好友"
            });
        }

        // 判断对方已发送给自己好友请求，不能再向对方发送
        bool bFriendRequested = await Program.db.Queryable<FriendRequestDO>()
            .WhereClassByPrimaryKey(new FriendRequestDO
            {
                requested_user_id = selfID,
                requester_user_id = otherID
            })
            .AnyAsync();
        if (bFriendRequested)
        {
            return Ok(new AddFriendSC
            {
                success = false,
                info = "对方已发送好友请求，不能再向对方发送"
            });
        }

        // 判断已发送过好友请求
        bool bAlreadyFriendRequesting = await Program.db.Queryable<FriendRequestDO>()
            .WhereClassByPrimaryKey(new FriendRequestDO
            {
                requested_user_id = otherID,
                requester_user_id = selfID
            })
            .AnyAsync();
        // 不存在就添加
        if (!bAlreadyFriendRequesting)
        {
            await Program.db.Insertable(new FriendRequestDO
            {
                requested_user_id = otherID,
                requester_user_id = selfID
            }).ExecuteCommandAsync();
        }

        return Ok(new AddFriendSC
        {
            success = true
        });
    }
}