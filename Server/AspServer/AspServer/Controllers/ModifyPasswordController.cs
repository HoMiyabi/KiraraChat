using AspServer.Models;
using Microsoft.AspNetCore.Mvc;

namespace AspServer.Controllers;

[ApiController]
public class ModifyPasswordController : Controller
{
    [HttpPost("modifypassword")]
    public async Task<IActionResult> Index([FromBody] ModifyPasswordCS modifyPasswordCS)
    {
        if (!Identity.Authenticate(modifyPasswordCS.sessionID, out int userID))
        {
            return Ok(new ModifyPasswordSC
            {
                success = false,
                info = "未登录"
            });
        }

        if (!Validator.Password(modifyPasswordCS.oldPassword) ||
            !Validator.Password(modifyPasswordCS.newPassword))
        {
            return Ok(new ModifyPasswordSC
            {
                success = false,
                info = "密码格式错误"
            });
        }

        UserTable userData = await Program.db.Queryable<UserTable>().InSingleAsync(userID);

        if (!PasswordHelper.ValidatePassword(modifyPasswordCS.oldPassword, userData.salt_hex, userData.password_salt_sha512_hex))
        {
            return Ok(new ModifyPasswordSC
            {
                success = false,
                info = "旧密码错误"
            });
        }

        if (modifyPasswordCS.oldPassword == modifyPasswordCS.newPassword)
        {
            return Ok(new ModifyPasswordSC
            {
                success = false,
                info = "新旧密码重复"
            });
        }

        string newSaltHex = PasswordHelper.GenSaltHex();
        userData.password_salt_sha512_hex = PasswordHelper.GetPasswordSaltSha512Hex(modifyPasswordCS.newPassword, newSaltHex);

        await Program.db.Updateable(userData)
            .UpdateColumns(it => new { it.password_salt_sha512_hex, it.salt_hex })
            .ExecuteCommandAsync();

        return Ok(new ModifyPasswordSC
        {
            success = true
        });
    }
}