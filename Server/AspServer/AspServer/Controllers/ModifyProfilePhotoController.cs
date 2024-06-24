using AspServer.Models;
using Microsoft.AspNetCore.Mvc;

namespace AspServer.Controllers;

[ApiController]
public class ModifyProfilePhotoController : Controller
{
    static List<string> profilePhotoNames = new()
    {
        "default", "elysia", "hop", "zhi"
    };
    
    [HttpPost("modifyprofilephoto")]
    public async Task<IActionResult> Index([FromBody] ModifyProfilePhotoCS modifyProfilePhotoCS)
    {
        if (!Identity.Authenticate(modifyProfilePhotoCS.sessionID, out int userID))
        {
            return Ok(new ModifyProfilePhotoSC
            {
                success = false,
                info = "未登录"
            });
        }

        if (!profilePhotoNames.Contains(modifyProfilePhotoCS.newProfilePhotoName))
        {
            return Ok(new ModifyProfilePhotoSC
            {
                success = false,
                info = "头像不存在"
            });
        }

        UserTable userData = await Program.db.Queryable<UserTable>().InSingleAsync(userID);

        userData.profile_photo_name = modifyProfilePhotoCS.newProfilePhotoName;

        await Program.db.Updateable(userData)
            .UpdateColumns(it => new { it.profile_photo_name })
            .ExecuteCommandAsync();

        return Ok(new ModifyProfilePhotoSC
        {
            success = true
        });
    }
}