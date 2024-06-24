using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

//public class UIWindow : UIView
//{
//    virtual public void UIOnCreate(Intent intent)
//    {
//    }

//    virtual public void UIOnDestroy()
//    {
//    }

//    virtual public void UIOnPause()
//    {
//    }

//    virtual public void UIOnResume()
//    {
//    }

//    virtual public void OnDestroy()
//    {
//        UIOnPause();
//        UIOnDestroy();
//    }

//    virtual public void UIFinish()
//    {

//        Destroy(gameObject);
//        //if (UIManager.Instance.windowStk.Count > 0)
//        //{
//        //    if (UIManager.Instance.windowStk.Peek() != this)
//        //    {
//        //        Debug.LogWarning("调用UIFinish的UIWindow非最顶层");
//        //    }

//        //    UIManager.Instance.windowStk.Pop();
//        //    if (UIManager.Instance.windowStk.Count > 0)
//        //    {
//        //        //UIManager.Instance.windowStk.Peek().gameObject.SetActive(true);
//        //        UIManager.Instance.windowStk.Peek().UIOnResume();
//        //    }
//        //}
//    }

//    public void UIStartWindow(Intent intent)
//    {
//        UIWindow uiWindow = Instantiate((GameObject)Resources.Load("UI/" + intent.windowName), UIManager.Instance.canvas.transform).GetComponent<UIWindow>();
//        //uiWindow.gameObject.SetActive(true);
//        uiWindow.UIOnCreate(intent);
//        uiWindow.UIOnResume();

//        //gameObject.SetActive(false);
//        UIOnPause();

//        UIManager.Instance.windowStk.Push(uiWindow);
//    }
//}