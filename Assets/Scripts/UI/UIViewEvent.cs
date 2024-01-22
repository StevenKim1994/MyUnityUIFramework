using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractUIViewEvent : IUIEvent
{
    public Action<IUIView> OnHideComplete;

    public virtual void ReleaseEvent()
    {
        OnHideComplete = null;
    }
}


