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
        ECSEvent.Create<OnErrorEvent>(gameObject, exception);
    }

    public void Connect() => client.enabled = true;
    public void Disconnect() => client.enabled = false;

    public void Send(string message) => client.Send(message);
    public void Send(byte[] bytes) => client.Send(bytes);
}
