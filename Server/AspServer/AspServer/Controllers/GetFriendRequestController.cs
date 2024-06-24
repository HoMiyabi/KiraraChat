using AspServer.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AspServer.Controllers;

[ApiController]
public class GetFriendRequestController : Controller
{
    [HttpPost("getfriendrequest")]
    public async Task<IActionResult> Index([FromBody] GetFriendRequestCS getFriendRequestC2S)
    {
        if (!Identity.Authenticate(getFriendRequestC2S.sessionID, out int userID))
        {
            return Ok(new GetFriendRequestSC
            {
                success = false,
                info = "未登录"
            });
        }

        List<ClientUserInfo> friendRequestUserInfos = await Program.db.Queryable<UserTable>()
            .InnerJoin<FriendRequestDO>((user, friendRequest) => user.id == friendRequest.requester_user_id)
            .Where((user, friendRequest) => friendRequest.requested_user_id == userID)
            .OrderBy(user => user.id)
            .Select(user => new ClientUserInfo
            {
                id = user.id,
                username = user.username,
                signature = user.signature,
                profilePhotoName = user.profile_photo_name
            })
            .ToListAsync();

        return Ok(new GetFriendRequestSC
        {
            success = true,
            clientUserInfos = friendRequestUserInfos
        });
    }
}