using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIToast : UIPanel
{
    public enum Length
    {
        Short, Long
    }

    [SerializeField] private Text uiText;
    
    public Length duration;

    public UIToast Init(string str, Length duration)
    {
        uiText.text = str;
        this.duration = duration;
        gameObject.SetActive(false);
        return this;
    }

    public static UIToast Make(string str, Length duration = Length.Short)
    {
        UIToast uIToast = UIManager.OpenPanel<UIToast>("Toast");
        uIToast.Init(str, duration);

        return uIToast;
    }

    public async void Show()
    {
        gameObject.SetActive(true);
        await Task.Delay(duration == Length.Short ? 1500 : 2500);
        Destroy(gameObject);
    }
}
