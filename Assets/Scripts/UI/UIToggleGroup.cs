using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ToggleGroup))]
public class UIToggleGroup : MonoBehaviour, IUIObject, IUIEnable
{
    private ToggleGroup UIGroup;
    private List<UIToggle> UIToggleList;
    private RectTransform UIRectTransform;
    private UIToggleGroupEvent UIEvent;

    private int SelectedIndex;

    public RectTransform Transform
    {
        get { return UIRectTransform; }
    }

    public UIToggleGroupEvent Event
    {
        get { return UIEvent; }
    }

    public int Index
    {
        get { return SelectedIndex; }
    }

    public bool Visible
    {
        get { return Transform.gameObject.activeInHierarchy; }
        set
        {
            Transform.gameObject.SetActive(value);
        }
    }

    public bool Enable
    {
        get 
        { 
            bool result = false;

            foreach (var toggle in UIToggleList)
            {
                if(toggle.Enable == false)  // 하나라도 비활성화되어있으면 비활성화함.
                {
                    result = toggle.Enable;
                    break;
                }
            }

            return result;
        }

        set
        {
            foreach (var toggle in UIToggleList)
            {
                toggle.Enable = value;
            }
        }
    }

    public bool IsBound(Vector2 ScreenPosition)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(Transform, ScreenPosition);
    }

    private void Awake()
    {
        UIRectTransform = GetComponent<RectTransform>();
        UIGroup = GetComponent<ToggleGroup>();

        UIToggleList = new List<UIToggle>();
        List<Toggle> toggles = new List<Toggle>(GetComponentsInChildren<Toggle>(true));
        for(int i = 0; i<toggles.Count;++i)
        {
            Toggle toggle = toggles[i];
            UIToggle extendUIToggle = toggle.AddComponent<UIToggle>();
            extendUIToggle.SetIndex(i);
            extendUIToggle.Event.OnSelectToggle = ChildToggleChangeValue;
            UIToggleList.Add(extendUIToggle);
        }

        UIEvent = new UIToggleGroupEvent();

        UIGroup.allowSwitchOff = true;

        if(UIToggleList.Count > 0)
        {
            UIToggleList[0].IsOn = true;
        }
        SelectedIndex = 0;
        UIGroup.allowSwitchOff = false;
    }

    private void ChildToggleChangeValue(int SelectIndex)
    {
        SelectedIndex = SelectIndex;
        Event.OnSelectIndex?.Invoke(SelectedIndex);
    }

    public void ResetToggleList()
    {
        for (int i = 0; i < UIToggleList.Count; ++i)
        {
            UIToggle toggle = UIToggleList[i];
            toggle.SetIsOnWithoutNotify(i == 0);
        }
        SelectedIndex = 0;
    }
}

public class UIToggleGroupEvent : IUIEvent
{
    public Action<int> OnSelectIndex;

    public void ReleaseEvent()
    {
        OnSelectIndex = null;
    }
}
