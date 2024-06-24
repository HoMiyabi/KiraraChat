using Cysharp.Threading.Tasks;
using Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIModifyProfilePhotoDialog : UIPanel
{
    public Button closeBtn;
    public Button useBtn;

    public Transform photos;

    public Image previewPhotoImg;
    public Text previewNameText;

    public string selectedPhotoName;

    public static UIModifyProfilePhotoDialog Make()
    {
        var dialog = UIManager.OpenPanel<UIModifyProfilePhotoDialog>("ModifyProfilePhotoDialog");
        dialog.Init();
        return dialog;
    }

    private UIModifyProfilePhotoDialog Init()
    {
        closeBtn.onClick.AddListener(UIClose);
        useBtn.onClick.AddListener(async () =>
        {
            closeBtn.interactable = false;
            useBtn.interactable = false;
            try
            {
                if (selectedPhotoName != LocalData.Instance.userInfo.profilePhotoName)
                {
                    await NetManager.ModifyProfilePhoto(selectedPhotoName);
                    LocalData.Instance.userInfo.profilePhotoName = selectedPhotoName;
                }
                UIClose();
            }
            catch (ApplicationException e)
            {
                Tools.LogToast(e.Message);
            }
            closeBtn.interactable = true;
            useBtn.interactable = true;
        });

        photos.DestroyAllChild();

        for (int i = 0; i < StaticData.Instance.soProfilePhotoAssets.spriteAssets.Count; i++)
        {
            var item = UIProfilePhotoSelectItem.Make(this, photos, StaticData.Instance.soProfilePhotoAssets.spriteAssets[i].sprite);
            item.idx = i;
            item.onSelectedCallback = OnItemSelected;
        }


        selectedPhotoName = LocalData.Instance.userInfo.profilePhotoName;
        SpriteAsset asset = StaticData.Instance.profilePhotoName_Asset[selectedPhotoName];
        SetPreview(asset);
        return this;
    }

    public void SetPreview(SpriteAsset asset)
    {
        previewPhotoImg.sprite = asset.sprite;
        previewNameText.text = asset.externalName;
    }

    private void OnItemSelected(int idx)
    {
        SpriteAsset asset = StaticData.Instance.soProfilePhotoAssets.spriteAssets[idx];
        selectedPhotoName = asset.internalName;
        SetPreview(asset);
    }
}