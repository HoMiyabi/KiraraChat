using System;
using UnityEngine;
using UnityEngine.UI;

public class UILoginRegister : UIPanel
{
    public Button settingsButton;

    private void Awake()
    {
        settingsButton.onClick.AddListener(() => UISettingsDialog.Make());
    }
}