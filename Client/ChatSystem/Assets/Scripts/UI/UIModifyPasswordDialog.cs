using System;
using UnityEngine.UI;

public class UIModifyPasswordDialog : UIPanel
{
    public Button closeBtn;
    public Button cancelBtn;
    public Button confirmBtn;

    public InputField oldPasswordInput;
    public InputField newPassword1Input;
    public InputField newPassword2Input;

    public static UIModifyPasswordDialog Make()
    {
        var dialog = UIManager.OpenPanel<UIModifyPasswordDialog>("ModifyPasswordDialog");
        dialog.Init();
        return dialog;
    }

    private UIModifyPasswordDialog Init()
    {
        closeBtn.onClick.AddListener(UIClose);
        cancelBtn.onClick.AddListener(UIClose);
        confirmBtn.onClick.AddListener(async () =>
        {
            closeBtn.interactable = false;
            cancelBtn.interactable = false;
            confirmBtn.interactable = false;
            try
            {
                if (newPassword1Input.text != newPassword2Input.text)
                {
                    throw new ApplicationException("两次密码输入不一致");
                }
                await NetManager.ModifyPassword(oldPasswordInput.text, newPassword1Input.text);
                UIToast.Make("修改成功").Show();
                UIClose();
            }
            catch (ApplicationException e)
            {
                Tools.LogToast(e.Message);
            }
            closeBtn.interactable = true;
            cancelBtn.interactable = true;
            confirmBtn.interactable = true;
        });
        return this;
    }
}