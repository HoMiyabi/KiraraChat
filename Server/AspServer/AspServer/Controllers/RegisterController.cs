using AspServer.Models;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using System.Diagnostics.Contracts;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace AspServer.Controllers;

[ApiController]
public class RegisterController : Controller
{
    [HttpPost("register")]
    public async Task<IActionResult> Index([FromBody] RegisterCS register)
    {
        if (!Validator.RegisterModel(register))
        {
            return Ok(new RegisterSC
            {
                success = false,
                info = "格式错误"
            });
        }

        string username = register.username;
        string password = register.password;

        // 查找用户名已存在
        bool usernameExist = await Program.db.Queryable<UserTable>()
            .Where(t => t.username == username).AnyAsync();
        if (usernameExist)
        {
            return Ok(new RegisterSC
            {
                success = false,
                info = "用户名已存在"
            });
        }

        (string saltHex, string passwordSaltSha512Hex) = PasswordHelper.SaltSha512Hex(password);

        UserTable userData = new()
        {
            username = username,
            password_salt_sha512_hex = passwordSaltSha512Hex,
            salt_hex = saltHex,
            profile_photo_name = "default"
        };

        // 返回的实体自增列会被设置，但有数据库默认值的列不会被设置，手动设置为default，有点丑
        UserTable userDataDb = await Program.db.Insertable(userData).IgnoreColumnsNull().ExecuteReturnEntityAsync();

        string sessionID = Identity.Give(userDataDb.id);

        return Ok(new RegisterSC
        {
            success = true,
            sessionID = sessionID,
            clientUserInfo = new()
            {
                id = userDataDb.id,
                username = userDataDb.username,
                signature = userDataDb.signature,
                profilePhotoName = userDataDb.profile_photo_name
            }
        });
    }
}

