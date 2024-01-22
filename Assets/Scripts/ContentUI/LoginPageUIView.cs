using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoginPageUIView : AbstractUIView<LoginUIViewProperty, LoginUIViewEvent>
{
    public override void Appearing()
    {
        string loginLocalID = string.Empty;
        if(PlayerPrefs.HasKey("LocalID"))
        {
            loginLocalID = PlayerPrefs.GetString("LocalID");
            UIViewProperty.LoginHashIDText.SetText(loginLocalID);
        }
        else
        {
            UIViewProperty.LoginHashIDText.SetText("계정생성필요");
        }
    }

    public override void Appeared()
    {

    }

    public override void Disappearing()
    {

    }

    public override void Disappeared()
    {

    }
}

public class LoginUIViewEvent : AbstractUIViewEvent
{
    public override void ReleaseEvent()
    {
        base.ReleaseEvent();
    }
}

public struct LoginUIViewProperty : UIViewProperty
{
    public TextMeshProUGUI LoginHashIDText;

    public void Initialize(RectTransform UIRectTransform)
    {
        LoginHashIDText = UIMapper.GetChildByPath<TextMeshProUGUI>(UIRectTransform, "/Background/AccountID Text");
    }

    public void Release()
    {
        LoginHashIDText = null;
    }
}
