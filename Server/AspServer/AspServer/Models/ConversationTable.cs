using SqlSugar;

namespace AspServer.Models;

[SugarTable("conversation")]
public class ConversationTable
{
    [SugarColumn(IsNullable = false, IsIdentity = true, IsPrimaryKey = true)]
    public int id { get; set; }

    [SugarColumn(IsNullable = false)]
    public int sender_id { get; set; }

    [SugarColumn(IsNullable = false)]
    public int receiver_id { get; set; }

    [SugarColumn(IsNullable = false)]
    public DateTime date_time { get; set; }

    [SugarColumn(IsNullable = false)]
    public int kind { get; set; }

    [SugarColumn(IsNullable = false, Length = 1024)]
    public string content { get; set; } = "";
}