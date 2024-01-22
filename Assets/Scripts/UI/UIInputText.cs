using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIInputText : MonoBehaviour, IUIObject, IPointerClickHandler
{
    private TMP_InputField TMPInputField;
    private RectTransform UIRectTransform;
    private UIInputTextEvent UIEvent;

    public RectTransform Transform
    {
        get
        {
            return UIRectTransform;
        }
    }

    public string CurrentInputText
    {
        get { return TMPInputField.text; }
    }

    public bool Enable
    {
        get { return TMPInputField.interactable; }
        set
        {
            TMPInputField.interactable = value;
        }
    }

    public UIInputTextEvent Event
    {
        get 
        { 
            return UIEvent; 
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

    private void Awake()
    {
        TMPInputField = GetComponent<TMP_InputField>();
        UIRectTransform = GetComponent<RectTransform>();
        UIEvent = new UIInputTextEvent();

        if(TMPInputField.onValueChanged.GetPersistentEventCount() > 0)
        {
            TMPInputField.onValueChanged.RemoveAllListeners();
        }
        if(TMPInputField.onSelect.GetPersistentEventCount() > 0 )
        {
            TMPInputField.onSelect.RemoveAllListeners();
        }

        TMPInputField.onValueChanged.AddListener((string inputString) =>
        {
            Event.OnChangeInputText?.Invoke(inputString);
        });

        TMPInputField.onSelect.AddListener((string currentString) =>
        {
            Event.OnPointClick?.Invoke(currentString);
        });
    }

    private void Start()
    {
    }

    public bool IsBound(Vector2 ScreenPosition)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(Transform, ScreenPosition);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Event.OnPointClick?.Invoke(TMPInputField.text);
    }

    public void SetText(string InputString)
    {
        TMPInputField.text = InputString;
    }

}

public class UIInputTextEvent : IUIEvent
{
    public Action<string> OnPointClick;
    public Action<string> OnChangeInputText;
    public void ReleaseEvent()
    {
        OnPointClick = null;
        OnChangeInputText = null;
    }
}
