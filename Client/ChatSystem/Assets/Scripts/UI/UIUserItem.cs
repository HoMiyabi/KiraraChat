using System;
using Models;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIUserItem : UIPanel
{
    [SerializeField] private Image phofilePhotoImg;
    [SerializeField] private Text usernameText;
    [SerializeField] private Button profilePhotoImgBtn;
    [SerializeField] private Button refuseBtn;
    [SerializeField] private Button acceptBtn;
    [SerializeField] private Button chatBtn;

    private ClientUserInfo userInfo;

    public UnityAction acceptOrRefuseCallback;

    public UIUserItem Init(ClientUserInfo userInfo, RelationshipToMe relationship)
    {
        this.userInfo = userInfo;
        usernameText.text = userInfo.username;

        phofilePhotoImg.sprite = StaticData.Instance.profilePhotoName_Asset[userInfo.profilePhotoName].sprite;

        refuseBtn.gameObject.SetActive(false);
        acceptBtn.gameObject.SetActive(false);
        chatBtn.gameObject.SetActive(false);

        switch (relationship)
        {
            case RelationshipToMe.Friend:
            {
                // ����
                chatBtn.gameObject.SetActive(true);
                chatBtn.onClick.AddListener(() =>
                {
                    UIChat.Make(userInfo);
                });

                // �Ҽ��˵�
                profilePhotoImgBtn.onClick.AddListener(() =>
                {
                    UIPopupMenu
                        .Make(profilePhotoImgBtn.transform.position)
                        .AddItem("�鿴����", () => UIUserDetailsDialog.Make(userInfo, relationship))
                        .AddItem("�Ƴ�����", () =>
                        {
                            var dialog = UIUnfriendDialog.Make(userInfo.id);
                            dialog.unfriendCallback = UIClose;
                        });
                });
            }
            break;

            //case RelationshipToMe.Stranger:
            //{
            //    //mainBtn.onClick.AddListener(() =>
            //    //{
            //    //    UIUserDetailsDialog.Make(userInfo, relationship);
            //    //});
            //}
            //break;

            case RelationshipToMe.FriendRequesting:
            {
                // ͬ��;ܾ���ť
                acceptBtn.gameObject.SetActive(true);
                refuseBtn.gameObject.SetActive(true);

                acceptBtn.onClick.AddListener(async () =>
                {
                    acceptBtn.interactable = false;
                    refuseBtn.interactable = false;
                    try
                    {
                        await NetManager.AcceptFriendRequest(userInfo.id);
                        UIToast.Make("�ѽ��ܺ�������").Show();
                        acceptOrRefuseCallback?.Invoke();
                        UIClose();
                    }
                    catch (ApplicationException e)
                    {
                        Tools.LogToast(e.Message);
                    }
                    acceptBtn.interactable = true;
                    refuseBtn.interactable = true;
                });

                refuseBtn.onClick.AddListener(async () =>
                {
                    acceptBtn.interactable = false;
                    refuseBtn.interactable = false;
                    try
                    {
                        await NetManager.RefuseFriendRequest(userInfo.id);
                        UIToast.Make("�Ѿܾ���������", UIToast.Length.Short).Show();
                        acceptOrRefuseCallback?.Invoke();
                        UIClose();
                    }
                    catch (ApplicationException e)
                    {
                        Tools.LogToast(e.Message);
                    }
                    acceptBtn.interactable = true;
                    refuseBtn.interactable = true;
                });

                // �Ҽ��˵�
                profilePhotoImgBtn.onClick.AddListener(() =>
                {
                    UIPopupMenu
                        .Make(profilePhotoImgBtn.transform.position)
                        .AddItem("�鿴����", () => UIUserDetailsDialog.Make(userInfo, relationship));
                });
            }
            break;

            default:
            {
                Debug.LogWarning("δ�����Relationship");
            }
            break;
        }

        return this;
    }

    public static UIUserItem Make(UIPanel uiRoot, Transform parent, ClientUserInfo userInfo, RelationshipToMe relationship)
    {

        UIUserItem item = UIManager.OpenPanel<UIUserItem>("UserItem", uiRoot, parent);
        item.Init(userInfo, relationship);
        return item;
    }
}
