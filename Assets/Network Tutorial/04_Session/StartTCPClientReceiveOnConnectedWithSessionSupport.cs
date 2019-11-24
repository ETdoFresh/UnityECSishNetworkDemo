using ECSish;
using System;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class StartTCPClientReceiveOnConnectedWithSessionSupport : MonoBehaviourSystem
{
    public string receivedMessage;

    private void Update()
    {
        foreach (var entity in GetEntities<OnConnectedEvent, TCPClient>())
        {
            var client = entity.Item2;
            if (!client) continue;
            if (!client.isConnected) continue;
            client.socket.BeginReceive(client.buffer, 0, client.buffer.Length, SocketFlags.None, OnReceive, client);
            client.isReceiving = true;
        }
    }

    private void OnReceive(IAsyncResult ar)
    {
        var client = (TCPClient)ar.AsyncState;
        try
        {
            var bytesRead = client.socket.EndReceive(ar);
            if (bytesRead == 0)
            {
                EventSystem.Add(() => client.gameObject.AddComponent<OnDisconnectedEvent>());
            }
            else
            {
                receivedMessage += Encoding.UTF8.GetString(client.buffer, 0, bytesRead);
                if (receivedMessage.Contains(Terminator.VALUE))
                {
                    var messages = receivedMessage.Split(new[] { Terminator.VALUE }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var message in messages)
                    {
                        EventSystem.Add(() =>
                        {
                            if (!client) return null;
                            var onReceiveEvent = client.gameObject.AddComponent<OnReceiveEvent>();
                            onReceiveEvent.message = message;
                            return onReceiveEvent;
                        });

                        var args = message.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                        if (int.TryParse(args[0], out int sessionId))
                        {
                            EventSystem.Add(() =>
                            {
                                var onReceiveEvent = client.gameObject.AddComponent<OnReceiveFromSessionEvent>();
                                onReceiveEvent.sessionId = sessionId;
                                onReceiveEvent.message = message;
                                onReceiveEvent.args = args;
                                return onReceiveEvent;
                            });
                        }
                    }

                    while (receivedMessage.Contains(Terminator.VALUE))
                        receivedMessage = receivedMessage.Substring(receivedMessage.IndexOf(Terminator.VALUE) + Terminator.VALUE.Length);
                }
                client.socket.BeginReceive(client.buffer, 0, client.buffer.Length, SocketFlags.None, OnReceive, client);
            }
        }
        catch
        {
            if (client.socket != null)
            {
                if (client.socket.Connected) client.socket.Disconnect(false);
                client.socket.Dispose();
                client.socket = null;
                EventSystem.Add(() => client.gameObject.AddComponent<OnDisconnectedEvent>());
            }
            client.isConnected = false;
            client.isReceiving = false;
        }
    }
}