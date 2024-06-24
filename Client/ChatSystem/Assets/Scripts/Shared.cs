using System.Collections.Generic;
using System;
using Validation;

public enum RelationshipToMe
{
    Self, Friend, Stranger, FriendRequesting
}

namespace Models
{
    [Serializable]
    public class ClientUserInfo
    {
        public int id;
        public string username;
        public string signature;
        public string profilePhotoName;
    }

    [Serializable]
    public class LoginCS
    {
        [Username]
        public string username;
        [Password]
        public string password;
    }

    [Serializable]
    public class LoginSC
    {
        public bool success;
        public string info;
        public string sessionID;
        public ClientUserInfo clientUserInfo;
    }

    [Serializable]
    public class RegisterCS
    {
        [Username]
        public string username;
        [Password]
        public string password;
    }

    [Serializable]
    public class RegisterSC
    {
        public bool success;
        public string info;
        public string sessionID;
        public ClientUserInfo clientUserInfo;
    }

    public class FriendCS
    {
        public string sessionID;
    }

    public class FriendSC
    {
        public bool success;
        public string info;
        public List<ClientUserInfo> clientUserInfos;
    }

    [Serializable]
    public class SearchUserCS
    {
        public string sessionID;
        [Username]
        public string username;
    }

    [Serializable]
    public class SearchUserSC
    {
        public bool success;
        public string info;
        public bool found;
        public RelationshipToMe relationshipToMe;
        public ClientUserInfo clientUserInfo;
    }

    [Serializable]
    public class AddFriendCS
    {
        public string sessionID;
        public int otherID;
    }

    [Serializable]
    public class AddFriendSC
    {
        public bool success;
        public string info;
    }

    [Serializable]
    public class GetFriendRequestCS
    {
        public string sessionID;
    }

    [Serializable]
    public class GetFriendRequestSC
    {
        public bool success;
        public string info;
        public List<ClientUserInfo> clientUserInfos;
    }   

    [Serializable]
    public class AcceptFriendRequestCS
    {
        public string sessionID;
        public int requesterID;
    }

    [Serializable]
    public class AcceptFriendRequestSC
    {
        public bool success;
        public string info;
    }

    [Serializable]
    public class RefuseFriendRequestCS
    {
        public string sessionID;
        public int requesterID;
    }

    [Serializable]
    public class RefuseFriendRequestSC
    {
        public bool success;
        public string info;
    }

    [Serializable]
    public class ModifySignatureCS
    {
        public string sessionID;
        public string newSignature;
    }

    [Serializable]
    public class ModifySignatureSC
    {
        public bool success;
        public string info;
    }

    [Serializable]
    public class ModifyPasswordCS
    {
        public string sessionID;
        [Password]
        public string oldPassword;
        [Password]
        public string newPassword;
    }

    [Serializable]
    public class ModifyPasswordSC
    {
        public bool success;
        public string info;
    }

    [Serializable]
    public class ModifyUsernameCS
    {
        public string sessionID;
        [Username]
        public string newUsername;
        [Password]
        public string password;
    }

    [Serializable]
    public class ModifyUsernameSC
    {
        public bool success;
        public string info;
    }

    [Serializable]
    public class ModifyProfilePhotoCS
    {
        public string sessionID;
        public string newProfilePhotoName;
    }

    [Serializable]
    public class ModifyProfilePhotoSC
    {
        public bool success;
        public string info;
    }


    public class UnfriendCS
    {
        public string sessionID;
        public int friendID;
    }

    public class UnfriendSC
    {
        public bool success;
        public string info;
    }

    public enum MessageKind { Text, Emoji }

    public class SendMessageCS
    {
        public string sessionID;
        public int receiverID;
        public MessageKind kind;
        public string content;
    }

    public class SendMessageSC
    {
        public bool success;
        public string info;
    }

    public class GetConversationCS
    {
        public string sessionID;
        public int friendID;
    }

    public class GetConversationSC
    {
        public bool success;
        public string info;
        public List<Message> messages;
    }

    [Serializable]
    public class Message
    {
        public int senderID;
        public int receiverID;
        public DateTime dateTime;
        public MessageKind messageKind;
        public string content;
    }

    public class PollMessageCS
    {
        public string sessionID;
        public int senderID;
    }

    public class PollMessageSC
    {
        public bool success;
        public string info;
        public bool timeout;
        public Message data;
    }
}