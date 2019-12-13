using CSharpNetworking;
using ECSish;
using System;
using UnityEngine;
using UnityNetworking;
using Object = UnityEngine.Object;

[RequireComponent(typeof(TCPClientUnity))]
public class TCPClient : MonoBehaviourComponentData
{
    [SerializeField] private TCPClientUnity client;

    private void OnValidate()
    {
        if (!client) client = GetComponent<TCPClientUnity>();
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
        EventSystem.Add(() => gameObject.AddComponent<OnConnectedEvent>());
    }

    private void OnClose(Object client)
    {
        EventSystem.Add(() => gameObject.AddComponent<OnDisconnectedEvent>());
    }

    private void OnMessage(Object server, Message message)
    {
        EventSystem.Add(() =>
        {
            var e = gameObject.AddComponent<OnReceiveEvent>();
            e.message = message.data;
            return e;
        });
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

    public void Send(string message) => client.Send(message);
    public void Send(byte[] bytes) => client.Send(bytes);
}
