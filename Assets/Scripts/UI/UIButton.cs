using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;


public interface IUIButton : IUIObject , IUIEnable
{

}

public class UIButtonEvent : IUIEvent
{
    public Action OnPointUp; // 오브젝트 클릭이후 올라올때 (주로 터치 지속 시간 체크를 위한 ) 
    public Action OnPointDown; // 오브젝트 클릭 시작 ( 주로 터치 지속 시간 체크를 위한 )
    public Action OnPointClick; // 동일한 오브젝트에서 내렷다가 올라올떄
    public Action OnPointEnter; // 오브젝트로 포인터 접근시
    public Action OnPointExit; // 오브젝트 Enter 상태에서 빠져나갈때

    public virtual void ReleaseEvent()
    {
        OnPointUp = null;
        OnPointDown = null;
        OnPointClick = null;
        OnPointEnter = null;
        OnPointExit = null;
    }
}

[RequireComponent(typeof(Button))]
public abstract class AbstractUIButton<TEvent> : MonoBehaviour, IUIButton, ISelectHandler, IDeselectHandler, IPointerUpHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler where TEvent : UIButtonEvent, new()
{
    private RectTransform UIRectTransform;
    private TEvent UIEvent;
    private Button UIButton;
    private List<Selectable> UISelectableList; // 하위에 상위 버튼에 따라 이미지 색상이 별도로 바뀌어야한다면 해당 자식컴포넌트에 Selectable 추가하면됨.

    protected virtual void Awake()
    {
        UISelectableList = new List<Selectable>(GetComponentsInChildren<Selectable>(true));
        UIEvent = new TEvent();
        UIButton = GetComponent<Button>();
        UIRectTransform = UIButton.transform as RectTransform;
    }

    public RectTransform Transform
    {
        get
        {
            return UIRectTransform;
        }
    }

    public TEvent Event
    {
        get { return UIEvent; }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UIButton.OnPointerEnter(eventData);
        for (int i = 0; i < UISelectableList.Count; ++i)
        {
            UISelectableList[i].OnPointerEnter(eventData);
        }

        if(UIButton.interactable)
        {
            Event.OnPointEnter?.Invoke();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIButton.OnPointerExit(eventData);
        for (int i = 0; i < UISelectableList.Count; ++i)
        {
            UISelectableList[i].OnPointerExit(eventData);
        }

        if(UIButton.interactable)
        {
            Event.OnPointExit?.Invoke();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        UIButton.OnPointerUp(eventData);
        for(int i = 0; i<UISelectableList.Count;++i)
        {
            UISelectableList[i].OnPointerUp(eventData);
        }
        
        if(UIButton.interactable)
        {
            Event.OnPointUp?.Invoke();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        UIButton.OnPointerDown(eventData);
        for(int i =0; i<UISelectableList.Count;++i)
        {
            UISelectableList[i].OnPointerDown(eventData);
        }

        if(UIButton.interactable)
        {
            Event.OnPointDown?.Invoke();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        UIButton.OnPointerClick(eventData);

        if(UIButton.interactable)
        {
            Event.OnPointClick?.Invoke();
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        for(int i = 0; i<UISelectableList.Count;++i)
        {
            UISelectableList[i].OnSelect(eventData);
        }
    }

    public void OnDeselect(BaseEventData eventData)
    {
        for (int i = 0; i < UISelectableList.Count; ++i)
        {
            UISelectableList[i].OnDeselect(eventData);
        }
    }

    // 하위 상속받은 클래스에서 기본 구현한 외에 추가적으로 오버라이딩하여 사용할수 있도록 virtual 로 구성
    public virtual bool IsBound(Vector2 ScreenPosition)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(UIRectTransform, ScreenPosition);
    }

    public virtual bool Visible 
    {
        get 
        { 
            return UIRectTransform.gameObject.activeInHierarchy; // 부모가 꺼져있을때는 자신도 꺼져있으므로 로직상 에러를 회피시키기 위해서 activeSelf대신 activeInHierarchy 사용
        }
        set
        {
            UIRectTransform.gameObject.SetActive(value);
        }
    }

    public virtual bool Enable
    {
        get
        {
            return UIButton.interactable;
        }

        set
        {
            UIButton.interactable = value;
            for(int i = 0; i<UISelectableList.Count;++i)
            {
                UISelectableList[i].interactable = value;
            }
        }
    }

    public virtual void OnDestroy()
    {
        UIEvent.ReleaseEvent();
        UIEvent = null;
        UIRectTransform = null;
        UISelectableList.Clear();
        UISelectableList = null;
    }
}

public class UIButton : AbstractUIButton<UIButtonEvent>
{
    private UIText UITMP;

    public UIText Text { get => UITMP; }

    protected override void Awake()
    {
        base.Awake();
        UITMP = UIMapper.GetChildByPath<UIText>(Transform, "Text (TMP)");
    }

    public void SetText(string InputString)
    {
        Text.SetText(InputString);
    }
}

