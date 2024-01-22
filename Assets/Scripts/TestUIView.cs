using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestUIViewProperty : UIViewProperty
{
    private Button Buttons;
    public Button Button1 { get => Buttons; }

    public void PropertyInit(RectTransform UIRectTransform)
    {
        Debug.Log("PropertyInit");
        Buttons = UIMapper.GetChildByPath<Button>(UIRectTransform, "Title"); 
    }
}

public class TestUIView : AbstractUIView<TestUIViewProperty, TestUIViewEvent>
{
    public override void Appeared()
    {
        print("Appear");
    }

    public override void Appearing()
    {
        print("Appearing");
        if (UIViewProperty.Button1)
        {
            print(UIViewProperty.Button1.name);
        }
    }

    public override void Disappeared()
    {
        print("Dissapear");
    }

    public override void Disappearing()
    {
        print("Disappearing");
    }
}

public class TestUIViewEvent : UIViewEvent
{
    public void ReleaseEvent()
    {

    }
}