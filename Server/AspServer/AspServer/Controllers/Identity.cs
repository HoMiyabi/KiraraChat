namespace AspServer.Controllers;

public class Identity
{
    private static Dictionary<string, int> sessionIDToUserID = new();

    public static bool Authenticate(string sessionID, out int userID)
    {
        return sessionIDToUserID.TryGetValue(sessionID, out userID);
    }

    public static string Give(int userID)
    {
        string sessionID = Guid.NewGuid().ToString("N");
        sessionIDToUserID.Add(sessionID, userID);
        return sessionID;
    }
}