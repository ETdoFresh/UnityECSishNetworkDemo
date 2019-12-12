using CSharpNetworking;
using ECSish;
using UnityEngine;
using UnityNetworking;

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
        Entity.Add(this);
        client.OnOpen.AddEditorListener(OnOpen);
        client.OnClose.AddEditorListener(OnClose);
        client.OnMessage.AddEditorListener(OnMessage);
    }

    private void OnDisable()
    {
        Entity.Remove(this);
        client.OnOpen.RemoveEditorListener(OnOpen);
        client.OnClose.RemoveEditorListener(OnClose);
        client.OnMessage.RemoveEditorListener(OnMessage);
    }

    private void OnOpen(Object client)
    {
        ECSEvent.Add(() => gameObject.AddComponent<OnConnectedEvent>());
    }

    private void OnClose(Object client)
    {
        ECSEvent.Add(() => gameObject.AddComponent<OnDisconnectedEvent>());
    }

    private void OnMessage(Object server, Message message)
    {
        ECSEvent.Add(() =>
        {
            var e = gameObject.AddComponent<OnReceiveEvent>();
            e.message = message.data;
            return e;
        });
    }

    public void Send(string message) => client.Send(message);
    public void Send(byte[] bytes) => client.Send(bytes);
}
