using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoSingleton<UIManager>
{
    public Canvas canvas;
    public string firstPanelName;

    // public Stack<UIPanel> panelStk = new();

    private void Start()
    {
        if (firstPanelName == "")
        {
            Debug.LogWarning("firstPanelName == \"\"");
        }
        else
        {
            OpenPanel<UIPanel>(firstPanelName);
        }
    }

    public static T OpenPanel<T>(string panelName, UIPanel uiRoot = null, Transform parent = null) where T : UIPanel
    {
        GameObject go = Instantiate(Resources.Load<GameObject>("UI/" + panelName), parent != null ? parent : Instance.canvas.transform);
        T uiPanel = go.GetComponent<T>();

        uiPanel.uiRoot = uiRoot != null ? uiRoot : uiPanel;
        return uiPanel;
    }

    public static UIPanel StartPanel(string panelName, UIPanel uiRoot = null, Transform parent = null)
    {
        GameObject go = Instantiate(Resources.Load<GameObject>("UI/" + panelName), parent != null ? parent : Instance.canvas.transform);
        UIPanel uiPanel = go.GetComponent<UIPanel>();

        uiPanel.uiRoot = uiRoot != null ? uiRoot : uiPanel;
        return uiPanel;
    }
}