using CSharpNetworking;
using ECSish;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityNetworking;
using Object = UnityEngine.Object;

[RequireComponent(typeof(TCPServerUnity))]
public class TCPServer : MonoBehaviourComponentData
{
    public TCPServerUnity server;
    public List<SocketClientConnection> clients = new List<SocketClientConnection>();

    private void OnValidate()
    {
        if (!server) server = GetComponent<TCPServerUnity>();
    }

    private void OnEnable()
    {
        Entity.Add(this);
        server.OnServerOpen.AddEditorListener(OnServerOpen);
        server.OnServerClose.AddEditorListener(onServerClose);
        server.OnOpen.AddEditorListener(OnOpen);
        server.OnClose.AddEditorListener(OnClose);
        server.OnMessage.AddEditorListener(OnMessage);
    }

    private void OnDisable()
    {
        Entity.Remove(this);
        server.OnServerOpen.RemoveEditorListener(OnServerOpen);
        server.OnServerClose.RemoveEditorListener(onServerClose);
        server.OnOpen.RemoveEditorListener(OnOpen);
        server.OnClose.RemoveEditorListener(OnClose);
        server.OnMessage.RemoveEditorListener(OnMessage);
    }

    private void OnServerOpen(Object server)
    {
        EventSystem.Add(() => gameObject.AddComponent<OnListeningEvent>());
    }

    private void onServerClose(Object server)
    {
        EventSystem.Add(() => gameObject.AddComponent<OnStopListeningEvent>());
    }

    private void OnOpen(Object server, Socket client)
    {
        var connectionGameObject = gameObject.scene.NewGameObject();
        connectionGameObject.name = "Client Connection " + client.GetHashCode();
        var connection = connectionGameObject.AddComponent<SocketClientConnection>();
        connection.socket = client;
        connection.host = ((IPEndPoint)client.RemoteEndPoint).Address.ToString();
        connection.port = ((IPEndPoint)client.RemoteEndPoint).Port;
        connection.protocol = client.ProtocolType.ToString().ToUpper();
        clients.Add(connection);
        EventSystem.Add(() =>
        {
            var e = gameObject.AddComponent<OnAcceptedEvent>();
            e.connection = connection;
            return e;
        });
    }

    private void OnClose(Object server, Socket client)
    {
        var connection = clients.Where(c => c.socket == client).FirstOrDefault();
        if (connection == null) return;
        
        clients.Remove(connection);
        EventSystem.Add(() =>
        {
            var e = gameObject.AddComponent<OnDisconnectedFromServerEvent>();
            e.connection = connection;
            return e;
        });
        Entity.Destroy(connection.gameObject);
    }

    private void OnMessage(Object server, Message<Socket> clientMessage)
    {
        var connection = clients.Where(c => c.socket == clientMessage.client).FirstOrDefault();
        if (connection == null) return;

        EventSystem.Add(() =>
        {
            var e = connection.gameObject.AddComponent<OnReceiveEvent>();
            e.message = clientMessage.data;
            return e;
        });
    }
}
