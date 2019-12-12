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
    [SerializeField] private TCPServerUnity server;
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
        ECSEvent.Create<OnListeningEvent>(gameObject);
    }

    private void onServerClose(Object server)
    {
        ECSEvent.Create<OnStopListeningEvent>(gameObject);
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
        ECSEvent.Create<OnAcceptedEvent>(gameObject, connection);
    }

    private void OnClose(Object server, Socket client)
    {
        var connection = clients.Where(c => c.socket == client).FirstOrDefault();
        if (connection == null) return;
        
        clients.Remove(connection);
        ECSEvent.Create<OnDisconnectedFromServerEvent>(gameObject, connection);
        Entity.Destroy(connection.gameObject);
    }

    private void OnMessage(Object server, Message<Socket> clientMessage)
    {
        var connection = clients.Where(c => c.socket == clientMessage.client).FirstOrDefault();
        if (connection == null) return;

        ECSEvent.Create<OnReceiveEvent>(connection, clientMessage.data);
    }

    public void Send(Socket socket, string message) => server.Send(socket, message);
    public void Send(Socket socket, byte[] bytes) => server.Send(socket, bytes);
    public void Disconnect(Socket socket) => server.Disconnect(socket);
}
