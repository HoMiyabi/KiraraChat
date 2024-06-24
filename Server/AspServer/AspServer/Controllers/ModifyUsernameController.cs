using AspServer.Models;
using Microsoft.AspNetCore.Mvc;

namespace AspServer.Controllers;

[ApiController]
public class ModifyUsernameController : Controller
{
    [HttpPost("modifyusername")]
    public async Task<IActionResult> Index([FromBody] ModifyUsernameCS modifyUsernameCS)
    {
        if (!Identity.Authenticate(modifyUsernameCS.sessionID, out int userID))
        {
            return Ok(new ModifyUsernameSC
            {
                success = false,
                info = "未登录"
            });
        }

        if (!Validator.Password(modifyUsernameCS.password) ||
            !Validator.Username(modifyUsernameCS.newUsername))
        {
            return Ok(new ModifyUsernameSC
            {
                success = false,
                info = "格式错误"
            });
        }

        UserTable userData = await Program.db.Queryable<UserTable>().InSingleAsync(userID);
        if (!PasswordHelper.ValidatePassword(modifyUsernameCS.password, userData.salt_hex, userData.password_salt_sha512_hex))
        {
            return Ok(new ModifyUsernameSC
            {
                success = false,
                info = "密码错误"
            });
        }

        // 新旧重复
        if (userData.username == modifyUsernameCS.newUsername)
        {
            return Ok(new ModifyUsernameSC
            {
                success = false,
                info = "未改变"
            });
        }

        // 新的已存在
        bool bNewUsernameExist = await Program.db.Queryable<UserTable>()
            .Where(it => it.username == modifyUsernameCS.newUsername)
            .AnyAsync();
        if (bNewUsernameExist)
        {
            return Ok(new ModifyUsernameSC
            {
                success = false,
                info = "用户名已存在"
            });
        }

        userData.username = modifyUsernameCS.newUsername;
        await Program.db.Updateable(userData)
            .UpdateColumns(it => new { it.username })
            .ExecuteCommandAsync();

        return Ok(new ModifyUsernameSC
        {
            success = true
        });
    }
}