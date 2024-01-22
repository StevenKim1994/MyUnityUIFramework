using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MatchMakingInfoPageUIView : AbstractUIView<MatchMakingInfoPageUIViewProperty, MatchMakingInfoPageUIViewEvent>
{
    public override void Appearing()
    {
        
    }

    public override void Appeared()
    {
        UIViewProperty.BackButton.onClick.AddListener(OnClickBackButton);
    }

    public override void Disappearing()
    {
        UIViewProperty.BackButton.onClick.RemoveAllListeners();
    }

    public override void Disappeared()
    {

    }

    private void OnClickBackButton()
    {
        Event.OnBack?.Invoke();
    }
}

public class MatchMakingInfoPageUIViewEvent : AbstractUIViewEvent
{
    public Action OnBack;
    public override void ReleaseEvent()
    {
        base.ReleaseEvent();
        OnBack = null;
    }
}

public struct MatchMakingInfoPageUIViewProperty : UIViewProperty
{
    public Button BackButton;
    public TextMeshProUGUI InfoText;

    public void Initialize(RectTransform UIRectTransform)
    {
        BackButton = UIMapper.GetChildByPath<Button>(UIRectTransform, "/Background/Back");
        InfoText = UIMapper.GetChildByPath<TextMeshProUGUI>(UIRectTransform, "/Background/Image/Text (TMP)");
    }

    public void Release()
    {
        BackButton = null;
        InfoText = null;
    }
}


