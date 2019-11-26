using System;
using System.Net.Sockets;
using CSharpNetworking;
using ECSish;
using UnityEngine;
using Object = UnityEngine.Object;

public class ConvertUnityNetworkingEvent : MonoBehaviour
{
    public TCPServer tcpServer;
    public TCPClient tcpClient;
    public WebSocketClient webSocketClient;

    private void OnEnable()
    {
        tcpServer?.unityNetworking.OnOpen.AddEditorListener(OnOpen);
        tcpServer?.unityNetworking.OnMessage.AddEditorListener(OnMessage);
        tcpServer?.unityNetworking.OnClose.AddEditorListener(OnClose);
        tcpClient?.unityNetworking.OnOpen.AddEditorListener(OnOpen);
        tcpClient?.unityNetworking.OnMessage.AddEditorListener(OnMessage);
        tcpClient?.unityNetworking.OnClose.AddEditorListener(OnClose);
        webSocketClient?.unityNetworking.OnOpen.AddEditorListener(OnOpen);
        webSocketClient?.unityNetworking.OnMessage.AddEditorListener(OnMessage);
        webSocketClient?.unityNetworking.OnClose.AddEditorListener(OnClose);
    }

    private void OnDisable()
    {
        tcpClient?.unityNetworking.OnOpen.RemoveEditorListener(OnOpen);
        tcpClient?.unityNetworking.OnMessage.RemoveEditorListener(OnMessage);
        tcpClient?.unityNetworking.OnClose.RemoveEditorListener(OnClose);
        webSocketClient?.unityNetworking.OnOpen.RemoveEditorListener(OnOpen);
        webSocketClient?.unityNetworking.OnMessage.RemoveEditorListener(OnMessage);
        webSocketClient?.unityNetworking.OnClose.RemoveEditorListener(OnClose);
    }

    private void OnOpen(Object arg0, Socket arg1)
    {
        throw new NotImplementedException();
    }

    private void OnMessage(Object arg0, Message<Socket> arg1)
    {
        throw new NotImplementedException();
    }

    private void OnClose(Object arg0, Socket arg1)
    {
        throw new NotImplementedException();
    }

    private void OnOpen(Object arg0)
    {
        EventSystem.Add(() => gameObject.AddComponent<OnConnectedEvent>());
    }

    private void OnMessage(Object arg0, Message arg1)
    {
        EventSystem.Add(() => {
            var onReceiveEvent = gameObject.AddComponent<OnReceiveEvent>();
            onReceiveEvent.message = arg1.data;
            return onReceiveEvent;
        });
    }

    private void OnClose(Object arg0)
    {
        EventSystem.Add(() => gameObject.AddComponent<OnDisconnectedEvent>());
    }
}
