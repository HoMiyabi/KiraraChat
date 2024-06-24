using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIPopupMenuItem : UIPanel
{
    public Text uiText;
    public Button btn;

    public UIPopupMenuItem Init(string text, UnityAction onClickCall)
    {
        uiText.text = text;
        btn.onClick.AddListener(() =>
        {
            uiRoot.UIClose();
            onClickCall?.Invoke();
        });
        return this;
    }

    public static UIPopupMenuItem Make(UIPanel uiRoot, Transform parent, string text, UnityAction onClickCall)
    {
        UIPopupMenuItem item = UIManager.OpenPanel<UIPopupMenuItem>("PopupMenuItem", uiRoot, parent);
        item.Init(text, onClickCall);
        return item;
    }
}