using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(Toggle))]
public class UIToggle : MonoBehaviour, IUIObject, IUIEnable
{
    private const string ANIMATOR_SELECT_PARAM = "Select";
    private int ToggleIndex;
    private Toggle Toggle;
    private UIToggleEvent UIEvent;
    private RectTransform UIRectTransform;
    private Animator UIAnimator;
    public UIToggleEvent Event
    {
        get { return UIEvent; }
    }

    public RectTransform Transform
    {
        get { return UIRectTransform; }
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

    public bool Enable
    {
        get
        {
            return Toggle.interactable;
        }

        set
        {
            Toggle.interactable = value;
        }
    }

    public bool IsOn
    {
        get
        {
            return Toggle.isOn;
        }
        set
        {
            Toggle.isOn = value;
        }
    }
    private void Awake()
    {
        UIRectTransform = GetComponent<RectTransform>();
        Toggle = Transform.GetComponent<Toggle>();
        UIEvent = new UIToggleEvent();

        UIAnimator = GetComponent<Animator>();
        if(UIAnimator != null)
        {
            UIAnimator.keepAnimatorStateOnDisable = true; // GameObject Off되어도 상태 유지되도록.
        }
    }

    private void Start()
    {
        Toggle.onValueChanged.RemoveAllListeners();
        Toggle.onValueChanged.AddListener(OnChangeToggle);
        Toggle.isOn = false;
    }

    public void SetIndex(int index)
    {
        ToggleIndex = index;
    }

    public void SetGroup(ToggleGroup Group)
    {
        Toggle.group = Group;
    }

    private void OnChangeToggle(bool value)
    {
        if(UIAnimator != null)
        {
            UIAnimator.SetBool(ANIMATOR_SELECT_PARAM, value);
        }

        if (value)
        {
            Event?.OnSelectToggle(ToggleIndex);
        }
    }

    public void SetIsOnWithoutNotify(bool value) // onChangeToggle 콜백없이 값변경시킬때 사용
    {
        Toggle.SetIsOnWithoutNotify(value);
    }

    public bool IsBound(Vector2 ScreenPosition)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(Transform, ScreenPosition);
    }
}

public class UIToggleEvent : IUIEvent
{
    public Action<int> OnSelectToggle;
    public void ReleaseEvent()
    {
        OnSelectToggle = null;
    }
}
