using AspServer.Models;
using Microsoft.AspNetCore.Mvc;

namespace AspServer.Controllers;

[ApiController]
public class ModifySignatureController : Controller
{
    [HttpPost("modifysignature")]
    public async Task<IActionResult> Index([FromBody] ModifySignatureCS modifySignatureCS)
    {
        if (!Identity.Authenticate(modifySignatureCS.sessionID, out int userID))
        {
            return Ok(new ModifySignatureSC
            {
                success = false,
                info = "未登录"
            });
        }

        if (!Validator.Signature(modifySignatureCS.newSignature))
        {
            return Ok(new ModifySignatureSC
            {
                success = false,
                info = "签名格式错误"
            });
        }

        UserTable userData = await Program.db.Queryable<UserTable>().InSingleAsync(userID);
        userData.signature = modifySignatureCS.newSignature;
        await Program.db.Updateable(userData).UpdateColumns(it => new {it.signature}).ExecuteCommandAsync();

        return Ok(new ModifySignatureSC
        {
            success = true
        });
    }
}