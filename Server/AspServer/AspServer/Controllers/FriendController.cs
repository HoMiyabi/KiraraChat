using AspServer.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AspServer.Controllers;

[ApiController]
public class FriendController : Controller
{
    [HttpPost("friend")]
    public async Task<IActionResult> Index([FromBody] FriendCS friend)
    {
        if (!Identity.Authenticate(friend.sessionID, out int userID))
        {
            return Ok(new FriendSC
            {
                success = false,
                info = "未登录"
            });
        }

        List<ClientUserInfo> friendUserInfos = await Program.db.Queryable<UserTable>()
            .InnerJoin<FriendRelationshipDO>((user, relationship) => user.id == relationship.user_id2)
            .Where((user, relationship) => relationship.user_id1 == userID)
            .OrderBy(user => user.id)
            .Select(user => new ClientUserInfo
            {
                id = user.id,
                username = user.username,
                signature = user.signature,
                profilePhotoName = user.profile_photo_name
            })
            .ToListAsync();

        return Ok(new FriendSC
        {
            success = true,
            clientUserInfos = friendUserInfos
        });
    }
}