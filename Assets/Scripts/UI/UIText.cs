using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class UIText : MonoBehaviour, IUIObject, ISelectHandler, IDeselectHandler, IPointerUpHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler  
{
    private TextMeshProUGUI TMP;
    private RectTransform UIRectTransform;
    private Selectable ColorSelectable;
    private UIButton Parent;

    private void Awake()
    {
        TMP = GetComponent<TextMeshProUGUI>();
        UIRectTransform = GetComponent<RectTransform>();
        ColorSelectable = GetComponent<Selectable>();

        Parent = Transform.GetComponentInParent<UIButton>();
    }

    public RectTransform Transform
    {
        get
        {
            return UIRectTransform;
        }
    }

    public bool Visible
    {
        get
        {
            return UIRectTransform.gameObject.activeInHierarchy;
        }
        set
        {
            UIRectTransform.gameObject.SetActive(value);
        }
    }

    public void SetText(string InputText)
    {
        TMP.SetText(InputText);
    }

    public bool IsBound(Vector2 ScreenPosition)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(Transform, ScreenPosition);
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (ColorSelectable != null && TMP != null)
        {
            TMP.color = ColorSelectable.colors.selectedColor;
        }

        if (Parent != null)
        {
            if (Parent is ISelectHandler)
            {
                (Parent as ISelectHandler).OnSelect(eventData);
            }
        }
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (ColorSelectable != null && TMP != null)
        {
            TMP.color = ColorSelectable.colors.normalColor;
        }

        if (Parent != null)
        {
            if (Parent is IDeselectHandler)
            {
                (Parent as IDeselectHandler).OnDeselect(eventData);
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (ColorSelectable != null && TMP != null)
        {
            TMP.color = ColorSelectable.colors.normalColor;
        }

        Parent?.OnPointerUp(eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (ColorSelectable != null && TMP != null)
        {
            TMP.color = ColorSelectable.colors.pressedColor;
        }

        Parent?.OnPointerDown(eventData);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (ColorSelectable != null && TMP != null)
        {
            TMP.color = ColorSelectable.colors.highlightedColor;
        }

        Parent?.OnPointerEnter(eventData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (ColorSelectable != null && TMP != null)
        {
            TMP.color = ColorSelectable.colors.normalColor;
        }

        Parent?.OnPointerExit(eventData);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (ColorSelectable != null && TMP != null)
        {
            TMP.color = ColorSelectable.colors.selectedColor;
        }

        Parent?.OnPointerClick(eventData);
    }
}
