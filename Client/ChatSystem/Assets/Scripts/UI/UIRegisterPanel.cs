using UnityEngine;
using UnityEngine.UI;
using System;

public class UIRegisterPanel : UIPanel
{
    public InputField usernameInput;
    public InputField password1Input;
    public InputField password2Input;
    public Button registerAndLoginBtn;

    private void Awake()
    {
        registerAndLoginBtn.onClick.AddListener(RegisterAndLoginBtn_onClick);
    }

    private async void RegisterAndLoginBtn_onClick()
    {
        registerAndLoginBtn.interactable = false;
        try
        {
            if (password1Input.text != password2Input.text)
            {
                throw new ApplicationException("两次密码输入不一致");
            }
            var userInfo = await NetManager.RegisterAndLogin(usernameInput.text, password1Input.text);
            LocalData.Instance.userInfo = userInfo;
            UIFriend.Make();
        }
        catch (ApplicationException e)
        {
            Tools.LogToast(e.Message);
        }
        registerAndLoginBtn.interactable = true;
    }
}