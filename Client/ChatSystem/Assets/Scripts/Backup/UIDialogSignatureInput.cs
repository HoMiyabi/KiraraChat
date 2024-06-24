//using Cysharp.Threading.Tasks;
//using Cysharp.Threading.Tasks.Triggers;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Unity.VisualScripting;
//using UnityEngine;
//using UnityEngine.UI;


//public class UIDialogSignatureInput : UIPanel
//{
//    public Button closeBtn;
//    public Button cancelBtn;
//    public Button confirmBtn;

//    public InputField signatureInput;

//    private string originalSignature;

//    private void Awake()
//    {
//        closeBtn.onClick.AddListener(UIFinish);
//        cancelBtn.onClick.AddListener(UIFinish);

//        confirmBtn.onClick.AddListener(async () =>
//        {
//            closeBtn.interactable = false;
//            cancelBtn.interactable = false;
//            confirmBtn.interactable = false;
//            await ModifySignature();
//            closeBtn.interactable = true;
//            cancelBtn.interactable = true;
//            confirmBtn.interactable = true;
//        });
//    }

//    public UIDialogSignatureInput Init(string originalSignature)
//    {
//        this.originalSignature = originalSignature;
//        signatureInput.text = originalSignature;
//        return this;
//    }

//    private bool Validate()
//    {
//        string signautre = signatureInput.text;
//        string info;
//        if (!Validator.Signature(signautre, out info))
//        {
//            Debug.Log(info);
//            UIToast.Make(info, UIToast.Length.Short).Show();
//            return false;
//        }
//        return true;
//    }

//    private async UniTask ModifySignature()
//    {
//        if (!Validate()) { return; }
//        if (signatureInput.text == originalSignature)
//        {
//            UIToast.Make("未修改签名", UIToast.Length.Short).Show();
//            return;
//        }

//        // TODO))
//    }
//}