using AspServer.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AspServer.Controllers;

[ApiController]
public class AcceptFriendRequestController : Controller
{
    [HttpPost("acceptfriendrequest")]
    public async Task<IActionResult> Index([FromBody] AcceptFriendRequestCS acceptFriendRequestCS)
    {
        if (!Identity.Authenticate(acceptFriendRequestCS.sessionID, out int userID))
        {
            return Ok(new AcceptFriendRequestSC
            {
                success = false,
                info = "未登录"
            });
        }

        int requesterID = acceptFriendRequestCS.requesterID;

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

        // 添加到好友，双向添加
        await Program.db.Insertable(new List<FriendRelationshipDO>
        {
            new()
            {
                user_id1 = userID,
                user_id2 = requesterID
            },
            new()
            {
                user_id1 = requesterID,
                user_id2 = userID
            }
        }).ExecuteCommandAsync();

        return Ok(new AcceptFriendRequestSC
        {
            success = true
        });
    }
}