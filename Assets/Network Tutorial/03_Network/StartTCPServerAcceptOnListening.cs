using ECSish;
using System;
using System.Net;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartTCPServerAcceptOnListening : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var entity in GetEntities<OnListeningEvent, TCPServer>())
        {
            var server = entity.Item2;
            if (!server.isListening) continue;
            if (server.socket == null) continue;

            server.socket.BeginAccept(OnAccept, server);
            server.isAccepting = true;
        }
    }

    private void OnAccept(IAsyncResult ar)
    {
        var server = (TCPServer)ar.AsyncState;
        var newClientGameObject = (GameObject)null;
        var newClientConnection = (TCPClientConnection)null;
        try
        {
            EventSystem.Add(() =>
            {
                newClientGameObject = new GameObject("ClientConnection");
                SceneManager.MoveGameObjectToScene(newClientGameObject, gameObject.scene);
                newClientConnection = newClientGameObject.AddComponent<TCPClientConnection>();
                newClientConnection.socket = server.socket.EndAccept(ar);
                newClientConnection.host = ((IPEndPoint)newClientConnection.socket.RemoteEndPoint).Address.ToString();
                newClientConnection.port = ((IPEndPoint)newClientConnection.socket.RemoteEndPoint).Port;
                newClientConnection.connectionId = TCPClientConnection.nextId++;
                server.clients.Add(newClientConnection);
                var onAcceptedEvent = server.gameObject.AddComponent<OnAcceptedEvent>();
                onAcceptedEvent.connection = newClientConnection;
                return onAcceptedEvent;
            });
            server.socket.BeginAccept(OnAccept, server);
        }
        catch
        {
            server.clients.Remove(newClientConnection);
            if (newClientGameObject) Destroy(newClientGameObject);
            if (server.socket != null)
                server.socket.Close();
        }
    }
}
