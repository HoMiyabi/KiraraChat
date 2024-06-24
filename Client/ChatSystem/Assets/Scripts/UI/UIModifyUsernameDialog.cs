using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIModifyUsernameDialog : UIPanel
{
    public Mask mask;

    public Button closeBtn;
    public Button cancelBtn;
    public Button confirmBtn;

    public InputField passwordInput;
    public InputField newUsernameInput;

    private UIModifyUsernameDialog Init()
    {
        mask.btn.onClick.AddListener(UIClose);
        closeBtn.onClick.AddListener(UIClose);
        cancelBtn.onClick.AddListener(UIClose);

        confirmBtn.onClick.AddListener(async () =>
        {
            mask.btn.interactable = false;
            closeBtn.interactable = false;
            cancelBtn.interactable = false;
            confirmBtn.interactable = false;
            try
            {
                if (LocalData.Instance.userInfo.username == newUsernameInput.text)
                {
                    throw new ApplicationException("未改变");
                }
                await NetManager.ModifyUsername(newUsernameInput.text, passwordInput.text);
                UIToast.Make("修改成功").Show();
                LocalData.Instance.userInfo.username = newUsernameInput.text;
                UIClose();
            }
            catch (ApplicationException e)
            {
                Tools.LogToast(e.Message);
            }
            mask.btn.interactable = true;
            closeBtn.interactable = true;
            cancelBtn.interactable = true;
            confirmBtn.interactable = true;
        });

        newUsernameInput.text = LocalData.Instance.userInfo.username;

        return this;
    }

    public static UIModifyUsernameDialog Make()
    {
        UIModifyUsernameDialog dialog = UIManager.OpenPanel<UIModifyUsernameDialog>("ModifyUsernameDialog");
        dialog.Init();
        return dialog;
    }
}