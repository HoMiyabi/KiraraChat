using AspServer.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AspServer.Controllers;

[ApiController]
public class UnfriendController : Controller
{
    [HttpPost("unfriend")]
    public async Task<IActionResult> Index([FromBody] UnfriendCS unfriendCS)
    {
        if (!Identity.Authenticate(unfriendCS.sessionID, out int selfID))
        {
            return Ok(new UnfriendSC
            {
                success = false,
                info = "未登录"
            });
        }

        // 判断是否为好友
        bool bFriend = await Program.db.Queryable<FriendRelationshipDO>()
            .WhereClassByPrimaryKey(new FriendRelationshipDO
            {
                user_id1 = selfID,
                user_id2 = unfriendCS.friendID
            })
            .AnyAsync();
        if (!bFriend)
        {
            return Ok(new UnfriendSC
            {
                success = false,
                info = "不是好友"
            });
        }

        // 删除好友
        await Program.db.Deleteable<FriendRelationshipDO>(new List<FriendRelationshipDO>()
        { 
            new FriendRelationshipDO
            { 
                user_id1 = selfID, 
                user_id2 = unfriendCS.friendID
            },
            new FriendRelationshipDO 
            { 
                user_id1 = unfriendCS.friendID, 
                user_id2 = selfID
            }
        }).ExecuteCommandAsync();

        return Ok(new UnfriendSC
        {
            success = true
        });
    }
}