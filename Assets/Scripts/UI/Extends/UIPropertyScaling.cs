using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class UIEnteringScaling : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool IsEnter;
    private RectTransform UIRectTransform;

    public void OnPointerEnter(PointerEventData eventData)
    {
        IsEnter = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        IsEnter = false;
    }

    void Start()
    {
        UIRectTransform = GetComponent<RectTransform>();
        IsEnter = false;    
    }

    // Update is called once per frame
    void Update()
    {
        if(IsEnter)
        {
            UIRectTransform.DOScale(new Vector2(1.5f, 1.5f), 1.0f);
        }
        else
        {
            UIRectTransform.DOScale(Vector2.one, 1.0f);
        }
    }
}
