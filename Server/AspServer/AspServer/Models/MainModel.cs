using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design.Serialization;
using System.Security.Policy;

namespace AspServer.Models;

public enum RelationshipToMe
{
    Self, Friend, Stranger, FriendRequesting
}

public class ClientUserInfo
{
    public int id { set; get; }
    public string? username { set; get; }
    public string? signature { set; get; }
    public string? profilePhotoName { set; get; }
}

public class LoginCS
{
    public string? username { set; get; }
    public string? password { set; get; }
}

public class LoginSC
{
    public bool success { set; get; }
    public string? info { set; get; }
    public string sessionID { set; get; } = "";
    public ClientUserInfo? clientUserInfo { set; get; }
}

public class RegisterCS
{
    public string username { set; get; } = "";
    public string password { set; get; } = "";
}

public class RegisterSC
{
    public bool success { set; get; }
    public string? info { set; get; }
    public string sessionID { set; get; } = "";
    public ClientUserInfo? clientUserInfo { set; get; }
}

public class FriendCS
{
    public string sessionID { set; get; } = "";
}

public class FriendSC
{
    public bool success { set; get; }
    public string? info { set; get; }
    public List<ClientUserInfo> clientUserInfos { set; get; } = new();
}

public class SearchUserCS
{
    public string sessionID { set; get; } = "";
    public string username { set; get; } = "";
}

public class SearchUserSC
{
    public bool success { set; get; }
    public string? info { set; get; }
    public bool found { set; get; }
    public RelationshipToMe relationshipToMe { set; get; }
    public ClientUserInfo? clientUserInfo { set; get; }
}

public class AddFriendCS
{
    public string sessionID { set; get; } = "";
    public int otherID { set; get; }
}

public class AddFriendSC
{
    public bool success { set; get; }
    public string? info { set; get; }
}

public class GetFriendRequestCS
{
    public string sessionID { set; get; } = "";
}

public class GetFriendRequestSC
{
    public bool success { set; get; }
    public string? info { set; get; }
    public List<ClientUserInfo>? clientUserInfos { set; get; }
}

public class AcceptFriendRequestCS
{
    public string sessionID { set; get; } = "";
    public int requesterID { set; get; }
}

public class AcceptFriendRequestSC
{
    public bool success { set; get; }
    public string? info { set; get; }
}

public class RefuseFriendRequestCS
{
    public string sessionID { set; get; } = "";
    public int requesterID { set; get; }
}

public class RefuseFriendRequestSC
{
    public bool success { set; get; }
    public string? info { set; get; }
}

public class ModifySignatureCS
{
    public string sessionID { set; get; } = "";
    public string newSignature { set; get; } = "";
}

public class ModifySignatureSC
{
    public bool success { set; get; }
    public string? info { set; get; }
}

public class ModifyPasswordCS
{
    public string? sessionID { set; get; }
    public string? oldPassword { set; get; }
    public string? newPassword { set; get; }
}

public class ModifyPasswordSC
{
    public bool success { set; get; }
    public string? info { set; get; }
}

public class ModifyUsernameCS
{
    public string? sessionID { set; get; }
    public string? newUsername { set; get; }
    public string? password { set; get; }
}

public class ModifyUsernameSC
{
    public bool success { set; get; }
    public string? info { set; get; }
}

public class ModifyProfilePhotoCS
{
    public string? sessionID { set; get; }
    public string? newProfilePhotoName { set; get; }
}

public class ModifyProfilePhotoSC
{
    public bool success { set; get; }
    public string? info { set; get; }
}

public class UnfriendCS
{
    public string? sessionID { set; get; }
    public int friendID { set; get; }
}

public class UnfriendSC
{
    public bool success { set; get; }
    public string? info { set; get; }
}

public enum MessageKind { Text, Emoji }

public class SendMessageCS
{
    public string? sessionID { set; get; }
    public int receiverID { set; get; }
    public MessageKind kind { set; get; }
    public string? content { set; get; }
}

public class SendMessageSC
{
    public bool success { set; get; }
    public string? info { set; get; }
}

public class PollMessageCS
{
    public string? sessionID;
    public int senderID;
}

public class PollMessageSC
{
    public bool success;
    public string? info;
    public bool timeout;
    public Message? data;
}

public class GetConversationCS
{
    public string? sessionID { set; get; }
    public int friendID { set; get; }
}

public class GetConversationSC
{
    public bool success { set; get; }
    public string? info { set; get; }
    public List<Message>? messages { set; get; }
}

public class Message
{
    public int senderID { set; get; }
    public int receiverID { set; get; }
    public DateTime dateTime { set; get; }
    public MessageKind messageKind { set; get; }
    public string content { set; get; } = "";
}

public class CS<T>
{
    public string? sessionID;
    public T? data;
}

public class SC<T>
{
    public int code;
    public string? message;
    public T? data;
}

public class SessionID
{
    public string? sessionID;
}

public class FriendAndUnreadCount
{
    public ClientUserInfo? userInfo;
    public int unreadCount;
}

//public class ConversationCS
//{
//    public string sessionID { set; get; } = "";
//}

//public class ConversationMessage
//{
//    public int senderID { set; get; }
//    public int receiverID { set; get; }
//    public DateTime dateTime { set; get; }
//    public int kind { set; get; }
//    public string content { set; get; } = "";
//}