using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine;
using Models;

public class UIFriend : UIPanel
{
    public Button profilePhotoBtn;

    public Image profilePhotoImg;
    public Text usernameText;

    public UIFriend Init()
    {
        ClientUserInfo userInfo = LocalData.Instance.userInfo;
        usernameText.text = userInfo.username;

        profilePhotoImg.sprite = StaticData.Instance.profilePhotoName_Asset[userInfo.profilePhotoName].sprite;

        profilePhotoBtn.onClick.AddListener(() =>
        {
            UIPopupMenu.Make(profilePhotoBtn.transform.position)
            .AddItem("编辑资料", () => UIUserDetailsDialog.Make(userInfo, RelationshipToMe.Self))
            .AddItem("更换头像", () =>
            {
                var dialog = UIModifyProfilePhotoDialog.Make();
                dialog.uiFinishCallback = () =>
                {
                    profilePhotoImg.sprite = StaticData.Instance.profilePhotoName_Asset[userInfo.profilePhotoName].sprite;
                };
            })
            .AddItem("修改签名", () => UISignatureInputDialog.Make())
            .AddItem("修改用户名", () =>
            {
                var dialog = UIModifyUsernameDialog.Make();
                dialog.uiFinishCallback = () =>
                {
                    usernameText.text = userInfo.username;
                };
            })
            .AddItem("修改密码", () => UIModifyPasswordDialog.Make());
        });

        return this;
    }

    public static UIFriend Make()
    {
        UIFriend uiPanel = UIManager.OpenPanel<UIFriend>("Friend");
        uiPanel.Init();
        return uiPanel;
    }
}