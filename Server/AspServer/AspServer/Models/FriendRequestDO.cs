using SqlSugar;

namespace AspServer.Models;

[SugarTable("friend_request")]
public class FriendRequestDO
{
    [SugarColumn(IsNullable = false, IsPrimaryKey = true)]
    public int requested_user_id { get; set; }

    [SugarColumn(IsNullable = false, IsPrimaryKey = true)]
    public int requester_user_id { get; set; }
}