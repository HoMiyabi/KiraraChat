using System;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIUnfriendDialog : UIPanel
{
    public static string prefabName = "UnfriendDialog";

    public Text titleText;
    public Text messageText;

    public Button negativeButton;
    public Text negativeButtonText;

    public Button positiveButton;
    public Text positiveButtonText;

    public int friendID;

    public Action unfriendCallback;

    public static UIUnfriendDialog Make(int friendID)
    {
        UIUnfriendDialog dialog = UIManager.OpenPanel<UIUnfriendDialog>(prefabName);
        dialog.Init(friendID);
        return dialog;
    }

    public void Init(int friendID)
    {
        this.friendID = friendID;

        SetMessage("是否确认移除该好友？");
        SetNegativeButton("取消", UIClose);
        SetPositiveButton("确认", async () =>
        {
            negativeButton.interactable = false;
            positiveButton.interactable = false;
            try
            {
                await NetManager.Unfriend(this.friendID);
                unfriendCallback?.Invoke();
                UIClose();
            }
            catch (ApplicationException e)
            {
                Tools.WarnToast(e.Message);
            }
            negativeButton.interactable = true;
            positiveButton.interactable = true;
        });
    }

    public UIUnfriendDialog SetTitle(string text)
    {
        titleText.text = text;
        return this;
    }

    public UIUnfriendDialog SetMessage(string text)
    {
        messageText.text = text;
        return this;
    }

    public UIUnfriendDialog SetNegativeButton(string text, UnityAction onClickCallback)
    {
        negativeButtonText.text = text;
        negativeButton.onClick.AddListener(onClickCallback);
        return this;
    }

    public UIUnfriendDialog SetPositiveButton(string text, UnityAction onClickCallback)
    {
        positiveButtonText.text = text;
        positiveButton.onClick.AddListener(onClickCallback);
        return this;
    }
}
