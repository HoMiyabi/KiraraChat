using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UISelectEmojiItem : UIPanel, IPointerClickHandler
{
    public static string uiName = "SelectEmojiItem";
    public Image image;
    public Text nameText;

    public SpriteAsset spriteAsset;

    public UnityAction<UISelectEmojiItem> onSelectedCallback;

    private UISelectEmojiItem Init(SpriteAsset spriteAsset)
    {
        this.spriteAsset = spriteAsset;
        image.sprite = spriteAsset.sprite;
        nameText.text = spriteAsset.externalName;
        return this;
    }

    public static UISelectEmojiItem Make(UIPanel uiRoot, Transform parent, SpriteAsset spriteAsset)
    {
        return UIManager.OpenPanel<UISelectEmojiItem>(uiName, uiRoot, parent).Init(spriteAsset);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        onSelectedCallback?.Invoke(this);
    }
}