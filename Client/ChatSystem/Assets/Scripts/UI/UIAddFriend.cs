using Models;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAddFriend : UIPanel
{
    public InputField usernameInput;
    public Button searchBtn;

    public ScrollRect scrollRect;
    public Text noFriendRequestsInfoText;

    int friendRequestCount;

    public override void UIOnResume()
    {
        base.UIOnResume();
        UpdateFriendRequest();
    }

    private void Awake()
    {
        searchBtn.onClick.AddListener(SearchBtn_onClick);
    }

    private async void SearchBtn_onClick()
    {
        searchBtn.interactable = false;
        try
        {
            if (usernameInput.text == LocalData.Instance.userInfo.username)
            {
                throw new ApplicationException("不能搜索自己");
            }
            var (found, userInfo, relationshipToMe) = await NetManager.SearchUser(usernameInput.text);
            if (!found)
            {
                throw new ApplicationException("未找到");
            }
            UIUserDetailsDialog.Make(userInfo, relationshipToMe);
        }
        catch (ApplicationException e)
        {
            Tools.LogToast(e.Message);
        }
        searchBtn.interactable = true;
    }

    private void RefreshText()
    {
        noFriendRequestsInfoText.gameObject.SetActive(friendRequestCount == 0);
    }

    private async void UpdateFriendRequest()
    {
        List<ClientUserInfo> friendRequesterUserInfos = await NetManager.GetFriendRequest();

        Debug.Log("好友请求" + friendRequesterUserInfos.Count);
        friendRequestCount = friendRequesterUserInfos.Count;

        scrollRect.content.DestroyAllChild();

        foreach (ClientUserInfo clientUserInfo in friendRequesterUserInfos)
        {
            UIUserItem item = UIUserItem.Make(uiRoot, scrollRect.content, clientUserInfo, RelationshipToMe.FriendRequesting);
            item.acceptOrRefuseCallback = () =>
            {
                friendRequestCount--;
                RefreshText();
            };
        }
        RefreshText();
    }
}
