using AspServer;
using AspServer.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace AspServer.Controllers;

[ApiController]
public class LoginController : Controller
{
    [HttpPost("login")]
    public async Task<IActionResult> Index([FromBody] LoginCS login)
    {
        if (!Validator.LoginModel(login))
        {
            return Ok(new LoginSC
            {
                success = false,
                info = "格式错误"
            });
        }

        string username = login.username;
        string password = login.password;

        List<UserTable> userDataList =
            await Program.db.Queryable<UserTable>()
            .Where(t => t.username == username).ToListAsync();

        // 找不到用户
        if (userDataList.Count == 0)
        {
            return Ok(new LoginSC
            {
                success = false,
                info = "用户名或密码错误"
            });
        }

        UserTable userData = userDataList[0];

        // 验证密码
        if (!PasswordHelper.ValidatePassword(password, userData.salt_hex, userData.password_salt_sha512_hex))
        {
            return Ok(new LoginSC
            {
                success = false,
                info = "用户名或密码错误"
            });
        }

        string sessionID = Identity.Give(userData.id);

        return Ok(new LoginSC
        {
            success = true,
            sessionID = sessionID,
            clientUserInfo = new ClientUserInfo
            {
                id = userData.id,
                username = userData.username,
                signature = userData.signature,
                profilePhotoName = userData.profile_photo_name
            }
        });
    }
}

