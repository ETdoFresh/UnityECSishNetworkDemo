using CSharpNetworking;
using ECSish;
using System;
using UnityEngine;
using UnityNetworking;
using Object = UnityEngine.Object;

public class WebSocketClient : MonoBehaviourComponentData
{
    public WebSocketClientUnity client = new WebSocketClientUnity();

    private void OnValidate()
    {
        if (!client) client = GetComponent<WebSocketClientUnity>();
    }

    private void OnEnable()
    {
        client.OnOpen.AddEditorListener(OnOpen);
        client.OnClose.AddEditorListener(OnClose);
        client.OnMessage.AddEditorListener(OnMessage);
        client.OnError.AddEditorListener(OnError);
        Entity.Add(this);
    }

    private void OnDisable()
    {
        Entity.Remove(this);
        client.OnOpen.RemoveEditorListener(OnOpen);
        client.OnClose.RemoveEditorListener(OnClose);
        client.OnMessage.RemoveEditorListener(OnMessage);
        client.OnError.RemoveEditorListener(OnError);
    }

    private void OnOpen(Object client)
    {
        ECSEvent.Create<OnConnectedEvent>(gameObject);
    }

    private void OnClose(Object client)
    {
        ECSEvent.Create<OnDisconnectedEvent>(gameObject);
    }

    private void OnMessage(Object server, Message message)
    {
        ECSEvent.Create<OnReceiveEvent>(gameObject, message.data);
    }

    private void OnError(Object server, Exception exception)
    {
        EventSystem.Add(() =>
        {
            var e = gameObject.AddComponent<OnErrorEvent>();
            e.exception = exception;
            return e;
        });
    }
}

