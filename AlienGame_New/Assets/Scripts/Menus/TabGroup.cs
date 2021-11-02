using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabGroup : MonoBehaviour
{
    public List<TabButton> tabButtons;
    public List<GameObject> tabViews;
    public Sprite tabIdle;
    public Sprite tabHover;
    public Sprite tabActive;
    public TabButton selectedTab;
    
    public void Subscribe(TabButton btn)
    {
        if(tabButtons == null)
        {
            tabButtons = new List<TabButton>();
        }

        tabButtons.Add(btn);
    }

    public void OnTabEnter(TabButton btn)
    {
        ResetTabs();
        if(selectedTab == null || btn != selectedTab)
        {
            btn.selectedImg.sprite = tabHover;
        }

    }

    public void OnTabExit(TabButton btn)
    {
        ResetTabs();

    }

    public void OnTabSelected(TabButton btn)
    {
        if(selectedTab != null)
        {
            selectedTab.Deselect();
        }
        selectedTab = btn;
        selectedTab.Select();
        ResetTabs();
        btn.selectedImg.sprite = tabActive;
        int index = btn.transform.GetSiblingIndex();
        for(int i = 0; i < tabViews.Count; i++)
        {
            if(i == index)
            {
                tabViews[i].SetActive(true);
            }
            else
            {
                tabViews[i].SetActive(false);
            }
        }

    }

    public void ResetTabs()
    {
        foreach(TabButton btn in tabButtons)
        {
            if(selectedTab != null && btn == selectedTab){ continue; }
            btn.selectedImg.sprite = tabIdle;
        }
    }
}
