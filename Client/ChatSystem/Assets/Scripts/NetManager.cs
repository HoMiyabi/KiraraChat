using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using Cysharp.Threading.Tasks;
using Models;
using UnityEngine;
using Validation;

public class NetManager : MonoSingleton<NetManager>
{
    public HttpClient httpClient = new();
    public string sessionID;

    /// <summary>
    ///
    /// </summary>
    /// <param name="methodName"></param>
    /// <param name="parameter"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <exception cref="ApplicationException"></exception>
    public static async UniTask<T> RPCCallAsync<T>(
        string methodName, object parameter, CancellationToken cancellationToken = default)
    {
        try
        {
            HttpResponseMessage response = await Instance.httpClient.PostAsync
            (
                Settings.Instance.MainIPPort + "/" + methodName,
                new StringContent(
                    JsonUtility.ToJson(parameter),
                    Encoding.UTF8,
                    "application/json"
                ),
                cancellationToken
            );

            string responseBody = await response.Content.ReadAsStringAsync();
            var result = JsonUtility.FromJson<T>(responseBody);
            return result;
        }
        catch (HttpRequestException)
        {
            throw new ApplicationException("服务器连接失败");
        }
    }

    public static async UniTask<ClientUserInfo> Login(
        string username, string password)
    {
        var result = await RPCCallAsync<LoginSC>(
            "login",
            new LoginCS
            {
                username = username,
                password = password
            }.ValidateExc());

        if (!result.success)
        {
            throw new ApplicationException("登录失败：" + result.info);
        }
        Instance.sessionID = result.sessionID;
        return result.clientUserInfo;
    }


    public static async UniTask<ClientUserInfo> RegisterAndLogin(
        string username, string password)
    {
        var result = await RPCCallAsync<RegisterSC>(
            "register",
            new RegisterCS
            {
                username = username,
                password = password
            }.ValidateExc());

        if (!result.success)
        {
            throw new ApplicationException("注册失败：" + result.info);
        }
        Instance.sessionID = result.sessionID;
        return result.clientUserInfo;
    }

    public static async UniTask<List<ClientUserInfo>> GetFriendRequest()
    {
        var result = await RPCCallAsync<GetFriendRequestSC>(
            "getfriendrequest",
            new GetFriendRequestCS
            {
                sessionID = Instance.sessionID
            }.ValidateExc());

        if (!result.success)
        {
            throw new ApplicationException(result.info);
        }
        return result.clientUserInfos;
    }

    public static async UniTask<(bool found, ClientUserInfo userInfo, RelationshipToMe relationshipToMe)> SearchUser(
        string username)
    {
        var result = await RPCCallAsync<SearchUserSC>(
            "searchuser",
            new SearchUserCS
            {
                sessionID = Instance.sessionID,
                username = username
            }.ValidateExc());

        if (!result.success)
        {
            throw new ApplicationException(result.info);
        }
        return (result.found, result.clientUserInfo, result.relationshipToMe);
    }

    public static async UniTask AcceptFriendRequest(int requesterID)
    {
        var result = await RPCCallAsync<AcceptFriendRequestSC>(
            "acceptfriendrequest",
            new AcceptFriendRequestCS
            {
                sessionID = Instance.sessionID,
                requesterID = requesterID
            }.ValidateExc());

        if (!result.success)
        {
            throw new ApplicationException(result.info);
        }
    }

    public static async UniTask RefuseFriendRequest(int requesterID)
    {
        var result = await RPCCallAsync<RefuseFriendRequestSC>(
            "refusefriendrequest",
            new RefuseFriendRequestCS
            {
                sessionID = Instance.sessionID,
                requesterID = requesterID
            }.ValidateExc());

        if (!result.success)
        {
            throw new ApplicationException(result.info);
        }
    }

    public static async UniTask<List<ClientUserInfo>> Friend()
    {
        var result = await RPCCallAsync<FriendSC>(
            "friend",
            new FriendCS
            {
                sessionID = Instance.sessionID
            }.ValidateExc());

        if (!result.success)
        {
            throw new ApplicationException(result.info);
        }
        return result.clientUserInfos;
    }

    public static async UniTask ModifyPassword(string oldPassword, string newPassword)
    {
        var result = await RPCCallAsync<ModifyPasswordSC>(
            "modifypassword",
            new ModifyPasswordCS
            {
                sessionID = Instance.sessionID,
                oldPassword = oldPassword,
                newPassword = newPassword,
            }.ValidateExc());

        if (!result.success)
        {
            throw new ApplicationException(result.info);
        }
    }

    public static async UniTask ModifyUsername(string newUsername, string password)
    {
        var result =  await RPCCallAsync<ModifyUsernameSC>(
            "modifyusername",
            new ModifyUsernameCS
            {
                sessionID = Instance.sessionID,
                newUsername = newUsername,
                password = password
            }.ValidateExc());

        if (!result.success)
        {
            throw new ApplicationException(result.info);
        }
    }

    public static async UniTask ModifyProfilePhoto( string newProfilePhotoName)
    {
        var result = await RPCCallAsync<ModifyProfilePhotoSC>(
            "modifyprofilephoto",
            new ModifyProfilePhotoCS
            {
                sessionID = Instance.sessionID,
                newProfilePhotoName = newProfilePhotoName
            }.ValidateExc());

        if (!result.success)
        {
            throw new ApplicationException(result.info);
        }
    }

    public static async UniTask ModifySignature(string newSignature)
    {
        var result = await RPCCallAsync<ModifySignatureSC>(
            "modifysignature",
            new ModifySignatureCS
            {
                sessionID = Instance.sessionID,
                newSignature = newSignature
            }.ValidateExc());

        if (!result.success)
        {
            throw new ApplicationException(result.info);
        }
    }

    public static async UniTask Unfriend(int friendID)
    {
        var result = await RPCCallAsync<UnfriendSC>(
            "unfriend",
            new UnfriendCS
            {
                sessionID = Instance.sessionID,
                friendID = friendID
            }.ValidateExc());

        if (!result.success)
        {
            throw new ApplicationException(result.info);
        }
    }

    public static async UniTask AddFriend(int otherID)
    {
        var result = await RPCCallAsync<AddFriendSC>(
            "addfriend",
            new AddFriendCS
            {
                sessionID = Instance.sessionID,
                otherID = otherID
            }.ValidateExc());

        if (!result.success)
        {
            throw new ApplicationException(result.info);
        }
    }

    public static async UniTask<(bool timeout, Message message)>
        PollMessage(int senderID, CancellationToken token)
    {
        var result = await RPCCallAsync<PollMessageSC>(
            "pollmessage",
            new PollMessageCS
            {
                sessionID = Instance.sessionID,
                senderID = senderID
            }.ValidateExc(),
            token);

        if (!result.success)
        {
            throw new ApplicationException(result.info);
        }

        return (result.timeout, result.data);
    }

    public static async UniTask SendEmoji(
        int friendID, string emojiName)
    {
        var result = await RPCCallAsync<SendMessageSC>(
            "sendmessage",
            new SendMessageCS
            {
                sessionID = Instance.sessionID,
                receiverID = friendID,
                kind = MessageKind.Emoji,
                content = emojiName
            }.ValidateExc());

        if (!result.success)
        {
            throw new ApplicationException(result.info);
        }
    }

    public static async UniTask SendText(int friendID, string text)
    {
        var result = await RPCCallAsync<SendMessageSC>(
            "sendmessage",
            new SendMessageCS
            {
                sessionID = Instance.sessionID,
                receiverID = friendID,
                kind = MessageKind.Text,
                content = text
            }.ValidateExc());

        if (!result.success)
        {
            throw new ApplicationException(result.info);
        }
    }

    public static async UniTask<List<Message>> GetConversation(
        int friendID)
    {
        var result = await RPCCallAsync<GetConversationSC>(
            "getconversation",
            new GetConversationCS
            {
                sessionID = Instance.sessionID,
                friendID = friendID
            }.ValidateExc());

        if (!result.success)
        {
            throw new ApplicationException(result.info);
        }
        return result.messages;
    }
}
