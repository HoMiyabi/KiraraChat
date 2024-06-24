using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class UIPopupMenu : UIPanel
{
    public Button maskBtn;
    public RectTransform items;

    public UIPopupMenu Init(Vector3 follow)
    {
        Tools.DestroyAllChild(items);

        maskBtn.onClick.AddListener(UIClose);
        items.position = follow;
        return this;
    }

    public static UIPopupMenu Make(Vector3 follow)
    {
        UIPopupMenu menu = UIManager.OpenPanel<UIPopupMenu>("PopupMenu");
        menu.Init(follow);
        return menu;
    }

    public UIPopupMenu AddItem(string text, UnityAction onClickCall)
    {
        UIPopupMenuItem.Make(this, items, text, onClickCall);
        return this;
    }
}