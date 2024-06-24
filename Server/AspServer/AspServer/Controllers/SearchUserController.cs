using AspServer.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AspServer.Controllers;

[ApiController]
public class SearchUserController : Controller
{
    [HttpPost("searchuser")]
    public async Task<IActionResult> Index([FromBody] SearchUserCS searchUserC2S)
    {
        if (!Identity.Authenticate(searchUserC2S.sessionID, out int selfID))
        {
            return Ok(new SearchUserSC
            {
                success = false,
                info = "未登录"
            });
        }

        List<UserTable> userDataList = await Program.db.Queryable<UserTable>()
            .Where(it => it.username == searchUserC2S.username)
            .ToListAsync();

        if (userDataList.Count == 0)
        {
            return Ok(new SearchUserSC
            {
                success = true,
                found = false,
            });
        }
            
        UserTable otherData = userDataList[0];

        // 不能是自己
        if (selfID == otherData.id)
        {
            return Ok(new SearchUserSC
            {
                success = false,
                found = true,
                relationshipToMe = RelationshipToMe.Self
            });
        }

        // 判断是否为好友 好友是双向的
        bool bFriend = await Program.db.Queryable<FriendRelationshipDO>()
            .WhereClassByPrimaryKey(new FriendRelationshipDO
            {
                user_id1 = selfID,
                user_id2 = otherData.id
            })
            .AnyAsync();

        // 判断是否向我发送好友申请
        bool bFriendRequesting = await Program.db.Queryable<FriendRequestDO>()
            .WhereClassByPrimaryKey(new FriendRequestDO
            {
                requested_user_id = selfID,
                requester_user_id = otherData.id
            })
            .AnyAsync();

        ClientUserInfo clientUserInfo = new()
        {
            id = otherData.id,
            username = otherData.username,
            signature = otherData.signature,
            profilePhotoName = otherData.profile_photo_name
        };

        RelationshipToMe relationshipToMe = RelationshipToMe.Stranger;
        if (bFriend)
        {
            relationshipToMe = RelationshipToMe.Friend;
        }
        if (bFriendRequesting)
        {
            relationshipToMe = RelationshipToMe.FriendRequesting;
        }

        return Ok(new SearchUserSC
        {
            success = true,
            found = true,
            relationshipToMe = relationshipToMe,
            clientUserInfo = clientUserInfo
        });
    }
}