using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIProfilePhotoSelectItem : UIPanel, IPointerClickHandler
{
    public Transform border;
    public Transform bg;
    public Image photoImg;

    public UnityAction<int> onSelectedCallback;

    public int idx;

    public static UIProfilePhotoSelectItem Make(UIPanel uiRoot,  Transform parent, Sprite photo)
    {
        var item = 
            UIManager.OpenPanel<UIProfilePhotoSelectItem>("ProfilePhotoSelectItem", uiRoot, parent);
        item.Init(photo);
        return item;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        onSelectedCallback?.Invoke(idx);
    }

    private void Init(Sprite photo)
    {
        photoImg.sprite = photo;
    }
}