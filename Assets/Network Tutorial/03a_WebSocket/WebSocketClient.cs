using CSharpNetworking;
using ECSish;
using UnityEngine;
using UnityNetworking;

public class WebSocketClient : MonoBehaviourComponentData
{
    public WebSocketClientUnity client = new WebSocketClientUnity();

    private void OnValidate()
    {
        if (!client) client = GetComponent<WebSocketClientUnity>();
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
}

