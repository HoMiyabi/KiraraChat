using UnityEngine;
using UnityEngine.UI;
using System;

public class UILoginPanel : UIPanel
{
    [SerializeField] private InputField usernameInput;
    [SerializeField] private InputField passwordInput;
    [SerializeField] private Button loginBtn;

    private void Awake()
    {
        loginBtn.onClick.AddListener(LoginBtn_onClick);
    }

    private async void LoginBtn_onClick()
    {
        loginBtn.interactable = false;
        try
        {
            var userInfo = await NetManager.Login(usernameInput.text, passwordInput.text);
            LocalData.Instance.userInfo = userInfo;
            UIFriend.Make();
        }
        catch (ApplicationException e)
        {
            Tools.LogToast(e.Message);
        }
        loginBtn.interactable = true;
    }
}