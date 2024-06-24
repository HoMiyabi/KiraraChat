using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBack : UIPanel
{
    public Button backBtn;

    private void Awake()
    {
        backBtn.onClick.AddListener(() =>
        {
            uiRoot.UIClose();
        });
    }
}