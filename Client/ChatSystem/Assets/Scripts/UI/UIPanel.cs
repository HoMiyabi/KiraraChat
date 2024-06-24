using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIPanel : MonoBehaviour
{
    public UIPanel uiRoot;

    //public enum Kind
    //{
    //    Full, ModalIgnorable, ModalUnignorable, NotModal
    //}
    //public Kind kind;

    public UnityAction uiFinishCallback;

    public virtual void UIClose()
    {
        uiFinishCallback?.Invoke();
        Destroy(gameObject);
    }

    public virtual void UIOnPause()
    {

    }

    public virtual void UIOnResume()
    {

    }
}