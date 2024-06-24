using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UISettingsDialog : UIPanel
{
    public static string prefabName = "SettingsDialog";

    public InputField ipInput;
    public InputField portInput;
    public Dropdown resolutionDropdown;

    public Button negativeButton;
    public Button positionButton;

    public static UISettingsDialog Make()
    {
        var dialog = UIManager.OpenPanel<UISettingsDialog>(prefabName);
        dialog.Init();
        return dialog;
    }

    public int curIndex;

    private void Init()
    {
        ipInput.text = Settings.Instance.serverIP;
        portInput.text = Settings.Instance.mainPort.ToString();

        resolutionDropdown.ClearOptions();

        resolutionDropdown.AddOptions(
            Settings.Instance.availableResolution
            .Select(vec => $"{vec.x}x{vec.y}")
            .ToList());

        curIndex = Settings.Instance.resolutionIndex;
        resolutionDropdown.SetValueWithoutNotify(curIndex);
        resolutionDropdown.onValueChanged.AddListener(i =>
        {
            curIndex = i;
        });

        negativeButton.onClick.AddListener(UIClose);
        positionButton.onClick.AddListener(() =>
        {
            Settings.Instance.serverIP = ipInput.text;
            Settings.Instance.mainPort = int.Parse(portInput.text);
            if (curIndex != Settings.Instance.resolutionIndex)
            {
                var vec = Settings.Instance.availableResolution[curIndex];
                Screen.SetResolution(vec.x, vec.y, FullScreenMode.Windowed);
                Settings.Instance.resolutionIndex = curIndex;
            }
            UIClose();
        });
    }
}