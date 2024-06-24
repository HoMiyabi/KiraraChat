using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UITabItem : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public Graphic graphic;

    [Header("Unselected")]
    public Color unselectedNormalColor = new Color32(255, 255, 255, 255);
    public Color unselectedHighlightedColor = new Color32(242, 242, 242, 255);
    public Color unselectedPressedColor = new Color32(218, 218, 218, 255);

    [Header("Selected")]
    public Color selectedNormalColor = new Color32(242, 242, 242, 255);
    public Color selectedHighlightedColor = new Color32(230, 230, 230, 255);
    public Color selectedPressedColor = new Color32(207, 207, 207, 255);

    public float fadeDuration = 0.2f;

    public bool selected = false;
    public bool pressed = false;
    public bool inside = false;

    public string tabTitle;

    private Color targetColor;

    public Action<int> onClick;

    public int index;

    public void RefreshTargetColor()
    {
        Color newTargetColor;
        if (selected)
        {
            if (pressed)
            {
                newTargetColor = selectedPressedColor;
            }
            else if (inside)
            {
                newTargetColor = selectedHighlightedColor;
            }
            else
            {
                newTargetColor = selectedNormalColor;
            }
        }
        else
        {
            if (pressed)
            {
                newTargetColor = unselectedPressedColor;
            }
            else if (inside)
            {
                newTargetColor = unselectedHighlightedColor;
            }
            else
            {
                newTargetColor = unselectedNormalColor;
            }
        }
        if (newTargetColor != targetColor)
        {
            targetColor = newTargetColor;
            graphic.DOKill();
            graphic.DOColor(targetColor, fadeDuration);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        pressed = true;
        RefreshTargetColor();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pressed = false;
        RefreshTargetColor();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        onClick?.Invoke(index);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        inside = true;
        RefreshTargetColor();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        inside = false;
        RefreshTargetColor();
    }

    public void Select(UITabPanel tabPanel)
    {
        if (!selected)
        {
            selected = true;
            RefreshTargetColor();

            if (tabPanel.tabTitleText != null)
            {
                tabPanel.tabTitleText.text = tabTitle;
            }
        }
    }

    public void UnSelect()
    {
        if (selected)
        {
            selected = false;
            RefreshTargetColor();
        }
    }
}
