using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UINavigation : MonoBehaviour
{
    private Stack<IUIView> UIViewStack;
    private IUIView CurrentUIView;
    private Canvas TargetCanvas;

    private void Awake()
    {
        UIViewStack = new Stack<IUIView>();
        CurrentUIView = null;
    }

    public int Size()
    {
        if(UIViewStack != null)
        {
            return UIViewStack.Count;   
        }
        else
        {
            Debug.LogError(string.Format("UINavigation {0} : is Null" ,this.gameObject.name));
            return 0;
        }
    }

    public bool Contain(Type UIViewType)
    {
        foreach (var UIView in UIViewStack)
        {
            if(UIView.GetType().Equals(UIViewType))
            {
                return true;
            }
        }
        return false;
    }

    public IUIView CurrnetView()
    {
        IUIView topView = null;
        if(UIViewStack.Count > 0)
        {
            topView = CurrentUIView;
        }
        else
        {
            Debug.LogError("Empty Navigation UIView");
        }

        return topView;
    }

    public IUIView Pop() // 현재 켜져있는 UIView 제거, 이후 스택크기가 존재한다면 켜줌.
    {
        IUIView PopUIView = null;
        if(UIViewStack.Count > 0)
        {
            CurrentUIView = UIViewStack.Pop();
            CurrentUIView.Destroy();
        }

        if (UIViewStack.Count > 0)
        {
            PopUIView = UIViewStack.Peek();
            PopUIView.SetCanvas(TargetCanvas);
            PopUIView.Show();
            CurrentUIView = PopUIView;
        }
        return PopUIView;
    }

    public IUIView PopTo(Type type)
    {
        IUIView PopView = null;

        while(UIViewStack.Count > 0)
        {
            IUIView view = UIViewStack.Pop();
            if(view.GetType().Equals(type))
            {
                PopView = view;
                break;
            }
        }

        return PopView;
    }

    public void Push(IUIView PushUIView) // 생성될때 네비게이션에 등록
    {
        if(UIViewStack.Contains(PushUIView) == false)
        {
            UIViewStack.Push(PushUIView);
        }
        PushUIView.SetCanvas(TargetCanvas);
        PushUIView.Show();
        if (CurrentUIView != PushUIView)
        {
            CurrentUIView?.Hide();
        }
        CurrentUIView = PushUIView;
    }

    public void Clear()
    {
        foreach(var uiView in UIViewStack)
        {
            uiView.Destroy();
        }
        CurrentUIView = null;
        UIViewStack.Clear();
    }

    public IUIView SetActive(bool active)
    {
        if(CurrentUIView != null)
        {
            if(active)
            {
                CurrentUIView.Transform.gameObject.SetActive(true);
            }
            else
            {
                CurrentUIView.Transform.gameObject.SetActive(false);
            }
        }
        return CurrentUIView;
    }

    public void SetCanvas(Canvas TargetCanvas)
    {
        this.TargetCanvas = TargetCanvas;
    }
}
