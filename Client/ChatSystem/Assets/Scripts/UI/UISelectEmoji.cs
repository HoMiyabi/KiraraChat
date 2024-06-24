using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class UISelectEmoji : UIPanel
{
    public static string uiName = "SelectEmoji";
    public Transform content;

    public Transform follow;

    public Button maskBtn;

    public UnityAction<string> finishSelectCallback;

    private UISelectEmoji Init(Vector3 followPosition)
    {
        maskBtn.onClick.AddListener(UIClose);

        follow.position = followPosition;

        content.DestroyAllChild();
        foreach (SpriteAsset e in StaticData.Instance.soEmojiAssets.spriteAssets)
        {
            UISelectEmojiItem item = UISelectEmojiItem.Make(this, content, e);
            item.onSelectedCallback = ItemSelected;
        }
        return this;
    }

    public static UISelectEmoji Make(Vector3 followPosition)
    {
        return UIManager.OpenPanel<UISelectEmoji>(uiName).Init(followPosition);
    }

    private void ItemSelected(UISelectEmojiItem item)
    {
        UIClose();
        finishSelectCallback?.Invoke(item.spriteAsset.internalName);
    }
}