using System;
using Models;
using UnityEngine;
using UnityEngine.UI;

public class UIUserDetailsDialog : UIPanel
{
    public Image maskImg;
    public Button closeBtn;

    public Image profilePhotoImg;
    public Text usernameText;
    public Text signatureText;
    public Button signatureModifyBtn;
    public Button addFriendBtn;

    public bool CanceledOnTouchOutside { get; set; } = true; // todo))

    private ClientUserInfo clientUserInfo;

    private void Awake()
    {
        closeBtn.onClick.AddListener(UIClose);
    }

    public static UIUserDetailsDialog Make(ClientUserInfo clientUserInfo, RelationshipToMe relationship)
    {
        UIUserDetailsDialog panel = UIManager.OpenPanel<UIUserDetailsDialog>("UserDetailsDialog");
        panel.Init(clientUserInfo, relationship);
        return panel;
    }

    public void Init(ClientUserInfo clientUserInfo, RelationshipToMe relationship)
    {
        this.clientUserInfo = clientUserInfo;
        usernameText.text = clientUserInfo.username;
        signatureText.text = clientUserInfo.signature;


        profilePhotoImg.sprite = StaticData.Instance.profilePhotoName_Asset[clientUserInfo.profilePhotoName].sprite;

        addFriendBtn.gameObject.SetActive(false);


        switch (relationship)
        {
            case RelationshipToMe.Friend:
            {
            }
            break;

            case RelationshipToMe.Stranger:
            {
                addFriendBtn.gameObject.SetActive(true);
                addFriendBtn.onClick.AddListener(async () =>
                {
                    addFriendBtn.interactable = false;
                    try
                    {
                        await NetManager.AddFriend(this.clientUserInfo.id);
                        Tools.LogToast("好友请求发送成功");
                    }
                    catch (ApplicationException e)
                    {
                        Tools.LogToast(e.Message);
                    }
                    addFriendBtn.interactable = true;
                });
            }
            break;

            case RelationshipToMe.Self:
            {
                signatureModifyBtn.onClick.AddListener(() =>
                {
                    var dialog = UISignatureInputDialog.Make();
                    dialog.uiFinishCallback = () =>
                    {
                        signatureText.text = LocalData.Instance.userInfo.signature;
                    };
                });
            }
            break;

            case RelationshipToMe.FriendRequesting:
            {
            }
            break;

            default:
            {
                Debug.LogWarning("未处理的Relationship");
            }
            break;
        }
    }
}
