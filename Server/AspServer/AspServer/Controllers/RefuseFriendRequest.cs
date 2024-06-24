using AspServer.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AspServer.Controllers;

[ApiController]
public class RefuseFriendRequest : Controller
{
    [HttpPost("refusefriendrequest")]
    public async Task<IActionResult> Index([FromBody] RefuseFriendRequestCS refuseFriendRequestCS)
    {
        if (!Identity.Authenticate(refuseFriendRequestCS.sessionID, out int userID))
        {
            return Ok(new AcceptFriendRequestSC
            {
                success = false,
                info = "未登录"
            });
        }

        int requesterID = refuseFriendRequestCS.requesterID;

        // 检查是否有好友请求
        bool bExistRequest = await Program.db.Queryable<FriendRequestDO>()
            .WhereClassByPrimaryKey(new FriendRequestDO
            {
                requested_user_id = userID,
                requester_user_id = requesterID
            })
            .AnyAsync();

        // 没有
        if (!bExistRequest)
        {
            return Ok(new AcceptFriendRequestSC
            {
                success = false,
                info = "不存在好友请求"
            });
        }
        // 有
        // 从请求中删除
        await Program.db.Deleteable<FriendRequestDO>(new FriendRequestDO
        {
            requested_user_id = userID,
            requester_user_id = requesterID
        }).ExecuteCommandAsync();

        return Ok(new AcceptFriendRequestSC
        {
            success = true
        });
    }
}