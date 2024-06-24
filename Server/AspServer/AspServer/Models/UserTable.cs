using SqlSugar;

namespace AspServer.Models;

[SugarTable(TableName = "user")]
public class UserTable
{
    [SugarColumn(IsNullable = false, IsIdentity = true, IsPrimaryKey = true)]
    public int id { get; set; }

    [SugarColumn(IsNullable = false, Length = 32)]
    public string? username { get; set; }

    [SugarColumn(IsNullable = false, Length = 128)]
    public string? password_salt_sha512_hex { get; set; }

    [SugarColumn(IsNullable = false, Length = 32)]
    public string? salt_hex { get; set; }

    [SugarColumn(IsNullable = false, Length = 32, DefaultValue = "''")]
    public string? signature { get; set; }

    [SugarColumn(IsNullable = false, Length = 32, DefaultValue = "default")]
    public string? profile_photo_name { get; set; }
}