using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MatchMakingScene : MonoBehaviour
{
    public Canvas MainCanvas;
    private UINavigation MainUINavigation;
    private UINavigation SubUINavigation;

    private MatchMakingConnectPageUIView MatchMakingPage;


    // Start is called before the first frame update
    void Start()
    {
        return;
        MainUINavigation = new GameObject("MainUINavigation").AddComponent<UINavigation>();
        SubUINavigation = new GameObject("SubUINavigation").AddComponent<UINavigation>();
        UINavigationController.GetInstance().Add("MainUINavigation", MainUINavigation);
        UINavigationController.GetInstance().Add("SubUINavigation", SubUINavigation);
        MainUINavigation.SetCanvas(MainCanvas);
        SubUINavigation.SetCanvas(MainCanvas);

// LoginUIView
        /*
        GameObject loginUI = Resources.Load<GameObject>("LoginUIView");
        LoginUIView loginUIView = GameObject.Instantiate(loginUI).AddComponent<LoginUIView>();
        loginUIView.Initialize(UIViewTweeningType.OutBack, UIViewTweeningType.OutBack);
        UINavigationController.GetInstance().GetNavigation("MainUINavigation").Push(loginUIView);
        IUIView currentView = UINavigationController.GetInstance().GetNavigation("MainUINavigation").CurrnetView();
        currentView.SetCanvas(MainCanvas);
        currentView.Show();
        */

        GameObject uiObj = Resources.Load<GameObject>("MatchServerConnectPage");
        MatchMakingPage = GameObject.Instantiate(uiObj).AddComponent<MatchMakingConnectPageUIView>();
        MatchMakingPage.Initialize(UIViewTweeningType.OutBack, UIViewTweeningType.OutBack);
        MatchMakingPage.Event.OnStartMatchMakingEvent = () =>
        {
            StartMatchMakingInfo();
        };
        UINavigationController.GetInstance().GetNavigation("MainUINavigation").Push(MatchMakingPage);
    }

    private void StartMatchMakingInfo()
    {
        UINavigationController.GetInstance().GetNavigation("MainUINavigation").SetActive(false);
        MatchMakingInfoPageUIView infoView = null;
        UINavigation subNavigation = UINavigationController.GetInstance().GetNavigation("SubUINavigation");
        if (SubUINavigation.Contain(typeof(MatchMakingInfoPageUIView)))
        {
            infoView = subNavigation.PopTo(typeof(MatchMakingInfoPageUIView)) as MatchMakingInfoPageUIView;
        }
        else
        {
            GameObject uiObj = Resources.Load<GameObject>("MatchMakingInfoPage");
            infoView = GameObject.Instantiate(uiObj).AddComponent<MatchMakingInfoPageUIView>();
            infoView.Initialize(UIViewTweeningType.OutBack, UIViewTweeningType.OutBack);
            infoView.Event.OnBack = () =>
            {
                UINavigationController.GetInstance().GetNavigation("SubUINavigation").SetActive(false);
                UINavigationController.GetInstance().GetNavigation("MainUINavigation").SetActive(true);
                UINavigationController.GetInstance().GetNavigation("MainUINavigation").CurrnetView().Show();
            };
        }
        UINavigationController.GetInstance().GetNavigation("SubUINavigation").Push(infoView);
    }
}
