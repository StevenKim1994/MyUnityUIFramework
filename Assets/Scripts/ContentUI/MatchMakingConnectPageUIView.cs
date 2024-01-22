using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BestHTTP.SocketIO3;
using BestHTTP.SocketIO3.Events;
using System;
using System.Text;
using Unity.VisualScripting;

public class MatchMakingConnectPageUIView : AbstractUIView<MatchMakingConnectPageUIViewProperty, MatchMakingConnectPageUIViewEvent>
{
    private SocketManager SocketManager;

    public override void Appeared()
    {
        UIViewProperty.ServerConnectButton.onClick.AddListener(SocketOpen);
        UIViewProperty.ServerURIInputField.onValueChanged.AddListener(CheckInValidURI);
        UIViewProperty.ServerURIInputField.onSelect.AddListener(DefaultInputString);
        UIViewProperty.ServerConnectButton.transform.GetComponentInChildren<TextMeshProUGUI>().SetText("Server Connect");

    }

    public override void Appearing()
    {

    }

    public override void Disappeared()
    {
    }

    public override void Disappearing()
    {
        UIViewProperty.ServerConnectButton.onClick.RemoveAllListeners();
        UIViewProperty.ServerURIInputField.onValueChanged.RemoveAllListeners();
        UIViewProperty.ServerURIInputField.onSelect.RemoveAllListeners();
        UIViewProperty.StartMatchMakingButton.onClick.RemoveAllListeners();
    }

    public override void Initialize(UIViewTweeningType AppearTweeningType, UIViewTweeningType DisappearTweeingType)
    {
        base.Initialize(AppearTweeningType, DisappearTweeingType);
    }

    // Connect Section
    private void SocketOpen()
    {
        Event.OnStartMatchMakingEvent?.Invoke();
        return;
        SocketManager.Open();
    }

    private void RequestReconnect()
    {
        SocketManager = null;
        if (Uri.IsWellFormedUriString(UIViewProperty.ServerURIInputField.text, UriKind.Absolute)) // 유효한 uri 양식이면 
        {
            SocketOptions socketOptions = new SocketOptions();
            socketOptions.AutoConnect = false;
            SocketManager = new SocketManager(new Uri(UIViewProperty.ServerURIInputField.text), socketOptions);
            SocketManager.Socket.On<ConnectResponse>(SocketIOEventTypes.Connect, OnConnected);
            SocketManager.Socket.On(SocketIOEventTypes.Disconnect, OnDisconnected);
            SocketManager.Socket.On<object>(SocketIOEventTypes.Error, OnConnectError);
            SocketManager.Socket.On<object>(SocketIOEventTypes.Unknown, OnUnknown);
            SocketManager.Socket.On<object>("welcome", OnConnectedWelcome);
            SocketManager.Socket.On<object>("startMatchMaking", OnStartMatchMaking);
            UIViewProperty.ServerConnectButton.interactable = true;
            SocketManager.Open();
        }
    }


    void OnConnected(ConnectResponse resp)
    {
        UIViewProperty.ServerResponseText.SetText(string.Format("ConnectResponseSID : {0}", resp.sid));
        UIViewProperty.ServerURIInputField.interactable = false;
        UIViewProperty.ServerConnectButton.onClick.RemoveAllListeners();
        UIViewProperty.ServerConnectButton.onClick.AddListener(RequestDisconnect);
        UIViewProperty.ServerConnectButton.transform.GetComponentInChildren<TextMeshProUGUI>().SetText(string.Format("SID: {0} \n Server Disconnect", resp.sid));

        UIViewProperty.StartMatchMakingButton.interactable = true;
        UIViewProperty.StartMatchMakingButton.onClick.AddListener(StartMatchMaking);
    }



    private void RequestDisconnect()
    {
        SocketManager.Close();
    }

    private void DefaultInputString(string arg0)
    {
        //172.20.18.153:3000 defaultServer
        UIViewProperty.ServerURIInputField.text = "http://172.20.18.153:3000";
    }

    private void CheckInValidURI(string InputValue)
    {
        if (Uri.IsWellFormedUriString(InputValue, UriKind.Absolute)) // 유효한 uri 양식이면 
        {
            SocketOptions socketOptions = new SocketOptions();
            socketOptions.AutoConnect = false;
            SocketManager = new SocketManager(new Uri(InputValue), socketOptions);
            SocketManager.Socket.On<ConnectResponse>(SocketIOEventTypes.Connect, OnConnected);
            SocketManager.Socket.On(SocketIOEventTypes.Disconnect, OnDisconnected);
            SocketManager.Socket.On<object>(SocketIOEventTypes.Error, OnConnectError);
            SocketManager.Socket.On<object>(SocketIOEventTypes.Unknown, OnUnknown);
            SocketManager.Socket.On<object>("welcome", OnConnectedWelcome);
            UIViewProperty.ServerConnectButton.interactable = true;
        }
        else
        {
            UIViewProperty.ServerConnectButton.interactable = false;
            UIViewProperty.ServerResponseText.SetText(string.Format("URI ERROR : InputValue: {0}", InputValue));
        }
    }

    private void OnUnknown(object Objects)
    {
        PrintSocketIOObjectString(Objects);
    }

    private void OnConnectError(object Objects)
    {
        PrintSocketIOObjectString(Objects);
    }

    private void OnDisconnected()
    {
        UIViewProperty.ServerResponseText.SetText("Disconnected");
        UIViewProperty.ServerURIInputField.interactable = true;
        UIViewProperty.ServerConnectButton.onClick.RemoveAllListeners();
        UIViewProperty.ServerConnectButton.onClick.AddListener(RequestReconnect);
        UIViewProperty.ServerConnectButton.transform.GetComponentInChildren<TextMeshProUGUI>().SetText("Server Connect");
        UIViewProperty.ServerURIInputField.text = "http://172.20.18.153:3000";
        UIViewProperty.StartMatchMakingButton.onClick.RemoveAllListeners();
        UIViewProperty.StartMatchMakingButton.interactable = false;
    }

    private void OnConnectedWelcome(object Objects)
    {
        PrintSocketIOObjectString(Objects);

        
    }

    private void PrintSocketIOObjectString(object Objects)
    {
        Dictionary<string, object> TempDic = Objects as Dictionary<string, object>;
        if (!object.ReferenceEquals(TempDic, null))
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var Dic in TempDic)
            {
                stringBuilder.Append(string.Format("{0} : {1}\n", Dic.Key, Dic.Value));
            }

            UIViewProperty.ServerResponseText.SetText(stringBuilder.ToString());
        }
        else
        {
            UIViewProperty.ServerResponseText.SetText("Response is null");
        }
    }

    // MatchMaking Section
    private void StartMatchMaking()
    {
        SocketManager.Socket.Emit("StartMatchMaking");
    }

    private void OnStartMatchMaking(object obj)
    {
        PrintSocketIOObjectString(obj);

        Event.OnStartMatchMakingEvent?.Invoke();
    }
}

public class MatchMakingConnectPageUIViewEvent : AbstractUIViewEvent
{
    public Action OnStartMatchMakingEvent;

    public override void ReleaseEvent()
    {
        base.ReleaseEvent();
        OnStartMatchMakingEvent = null;
    }
}

public struct MatchMakingConnectPageUIViewProperty : UIViewProperty
{
    public Button ServerConnectButton;
    public Button StartMatchMakingButton;
    public TMP_InputField ServerURIInputField;
    public TextMeshProUGUI ServerResponseText;

    public void Initialize(RectTransform UIRectTransform)
    {
        ServerConnectButton = UIMapper.GetChildByPath<Button>(UIRectTransform, "/Background/Server Connect UIButton");
        StartMatchMakingButton = UIMapper.GetChildByPath<Button>(UIRectTransform, "/Background/Start MatchMaking UIButton");
        ServerURIInputField = UIMapper.GetChildByPath<TMP_InputField>(UIRectTransform, "/Background/ServerURI InputField");
        ServerResponseText = UIMapper.GetChildByPath<TextMeshProUGUI>(UIRectTransform, "/Image/Text (TMP)");
    }

    public void Release()
    {
        ServerConnectButton = null;
        StartMatchMakingButton = null;
        ServerURIInputField = null;
        ServerResponseText = null;
    }
}
