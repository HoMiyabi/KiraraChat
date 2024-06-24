using Models;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMyFriend : UIPanel
{
    public ScrollRect scrollRect;

    public override void UIOnResume()
    {
        base.UIOnResume();

        UpdateFriendList();
    }

    private async void UpdateFriendList()
    {
        List<ClientUserInfo> friendInfos = await NetManager.Friend();
        Debug.Log("好友数量：" + friendInfos.Count);

        // 删除所有孩子
        Tools.DestroyAllChild(scrollRect.content);

        foreach (ClientUserInfo friendInfo in friendInfos)
        {
            UIUserItem item = UIUserItem.Make(uiRoot, scrollRect.content, friendInfo, RelationshipToMe.Friend);
        }
    }
}