using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class UITabPanel : UIPanel
{
    public int defaultSelect;
    public int currentSelect;

    public Text tabTitleText;

    [Serializable]
    public struct TabItemAndPage
    {
        public UITabItem tabItem;
        public UIPanel page;
    }

    public List<TabItemAndPage> tabItemAndPages;

    public void Select(int index)
    {
        tabItemAndPages[index].tabItem.Select(this);
        tabItemAndPages[index].page.gameObject.SetActive(true);
        tabItemAndPages[index].page.UIOnResume();
    }

    public void UnSelect(int index)
    {
        tabItemAndPages[index].tabItem.UnSelect();
        tabItemAndPages[index].page.gameObject.SetActive(false);
        tabItemAndPages[index].page.UIOnPause();
    }

    private void Awake()
    {
        currentSelect = defaultSelect;
        Select(currentSelect);

        for (int i = 0; i < tabItemAndPages.Count; i++)
        {
            int temp = i;
            tabItemAndPages[i].tabItem.index = temp;
            tabItemAndPages[i].tabItem.onClick = TabItem_onClick;
            if (i != currentSelect)
            {
                UnSelect(i);
            }
        }
    }

    private void TabItem_onClick(int index)
    {
        if (index != currentSelect)
        {
            UnSelect(currentSelect);
            Select(index);
            currentSelect = index;
        }
    }
}
