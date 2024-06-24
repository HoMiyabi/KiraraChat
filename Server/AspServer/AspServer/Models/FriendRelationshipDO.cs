using SqlSugar;

namespace AspServer.Models;

[SugarTable("friend_relationship")]
public class FriendRelationshipDO
{
    [SugarColumn(IsNullable = false, IsPrimaryKey = true)]
    public int user_id1 { get; set; }

    [SugarColumn(IsNullable = false, IsPrimaryKey = true)]
    public int user_id2 { get; set; }
}