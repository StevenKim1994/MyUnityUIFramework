using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum UIViewTweeningType
{
    Linear,
    OutBack,
}

public enum UIViewTweeningTransitionState
{
    IsAppearing,
    IsAppeared,
    IsDisAppearing,
    IsDisAppeared,
}

public interface IUIView
{
    public RectTransform Transform { get; }
    public void Show();
    public void Hide();
    void SetCanvas(Canvas TargetCanvas);
    void Initialize(UIViewTweeningType AppearTweeningType, UIViewTweeningType DisappearTweeingType);
    void Release();
    void Destroy();
}

public abstract class AbstractUIView<TUIViewProperty, TUIViewEvent> : MonoBehaviour, IUIView where TUIViewEvent : AbstractUIViewEvent, new() where TUIViewProperty : UIViewProperty, new()
{
    protected RectTransform UIRectTransform;
    private UIViewTweeningType AppearTweeningType;
    private UIViewTweeningType DisappearTweeingType;
    protected UIViewTweeningTransitionState TransitionState;
    protected TUIViewProperty UIViewProperty;
    protected TUIViewEvent UIViewEvent;

    private Tween Tweener;

    public abstract void Appearing();
    public abstract void Appeared();
    public abstract void Disappearing();
    public abstract void Disappeared();

    public RectTransform Transform
    {
        get { return UIRectTransform; }
    }

    public UIViewTweeningTransitionState Transition
    {
        get { return TransitionState; }
    }

    public TUIViewEvent Event
    {
        get { return UIViewEvent; }
    }

    public void SetCanvas(Canvas TargetCanvas)
    {
        Transform.SetParent(TargetCanvas.transform);
        Transform.localScale = Vector2.one;
        Transform.localPosition = Vector2.zero;
    }

    public void Show()
    {
        gameObject.SetActive(true);
        Tweener.Kill();
        Appearing();
        TransitionState = 0;
        UIRectTransform.localScale = Vector2.zero;
        switch(AppearTweeningType)
        {
            case UIViewTweeningType.Linear:
                {
                    Tweener = UIRectTransform.DOScale(Vector2.one, 1.0f).SetEase(Ease.Linear).OnComplete(ShowComplete);
                }
                break;
            case UIViewTweeningType.OutBack:
                {
                    Tweener = UIRectTransform.DOScale(Vector2.one, 0.35f).SetEase(Ease.OutBack).OnComplete(ShowComplete);
                }
                break;
            // TODO :: 추가되는 Appearing 연출 추가 부분
        }
    }

    private void ShowComplete()
    {
        TransitionState = UIViewTweeningTransitionState.IsAppeared;
        Appeared();
    }


    public void Hide()
    {
        Tweener.Kill();
        Disappearing();
        TransitionState = UIViewTweeningTransitionState.IsDisAppearing;
        switch(DisappearTweeingType)
        {
            case UIViewTweeningType.Linear:
                {
                    Tweener = UIRectTransform.DOScale(Vector2.zero, 1.0f).SetEase(Ease.Linear).OnComplete(HideComplete);
                }
                break;
            case UIViewTweeningType.OutBack:
                {
                    Tweener =UIRectTransform.DOScale(Vector2.zero, 0.35f).SetEase(Ease.OutBack).OnComplete(HideComplete);
                }
                break;

            // TODO :: 추가되는 Disapparing 연출 추가 부분
        }
    }

    private void HideComplete()
    {
        Disappeared();
        TransitionState = UIViewTweeningTransitionState.IsDisAppeared;
        gameObject.SetActive(false);
        UIViewEvent.OnHideComplete?.Invoke(this);
    }

    public virtual void Initialize(UIViewTweeningType AppearTweeningType, UIViewTweeningType DisappearTweeingType)
    {
        UIRectTransform = gameObject.GetComponent<RectTransform>();
        TransitionState = UIViewTweeningTransitionState.IsDisAppeared;
        UIViewProperty = new TUIViewProperty();
        UIViewEvent = new TUIViewEvent();
        this.AppearTweeningType = AppearTweeningType;
        this.DisappearTweeingType = DisappearTweeingType;

        UIViewProperty.Initialize(UIRectTransform);
        UIRectTransform.gameObject.SetActive(false);
    }

    public virtual void Release()
    {
        UIViewProperty.Release();
        UIViewEvent.ReleaseEvent();
    }

    public void OnDestroy()
    {
        Release();
    }

    public void Destroy()
    {
        Destroy(this.gameObject);
    }
}

