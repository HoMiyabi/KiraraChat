using Cysharp.Threading.Tasks;
using Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISignatureInputDialog : UIPanel
{
    public Button closeBtn;
    public Button cancelBtn;
    public Button confirmBtn;

    public InputField signatureInput;

    public UISignatureInputDialog Init()
    {
        closeBtn.onClick.AddListener(UIClose);
        cancelBtn.onClick.AddListener(UIClose);

        signatureInput.text = LocalData.Instance.userInfo.signature;

        confirmBtn.onClick.AddListener(async () =>
        {
            closeBtn.interactable = false;
            cancelBtn.interactable = false;
            confirmBtn.interactable = false;
            try
            {
                if (signatureInput.text == LocalData.Instance.userInfo.signature)
                {
                    Tools.LogToast("未改变");
                }
                else
                {
                    await NetManager.ModifySignature(signatureInput.text);
                    LocalData.Instance.userInfo.signature = signatureInput.text;
                    UIToast.Make("修改成功").Show();
                }
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

    public static UISignatureInputDialog Make()
    {
        UISignatureInputDialog dialog = UIManager.OpenPanel<UISignatureInputDialog>("SignatureInputDialog");
        dialog.Init();
        return dialog;
    }


}
