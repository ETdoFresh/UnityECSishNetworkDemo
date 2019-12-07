using ECSish;
using System;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class StartTCPClientConnectionReceiveOnAcceptedWithSessionSupport : MonoBehaviourSystem
{
    public string receivedMessage;

    private void Update()
    {
        foreach (var entity in GetEntities<OnAcceptedEvent, TCPServer>())
        {
            //var newClient = entity.Item1.connection;
            //newClient.socket.BeginReceive(newClient.buffer, 0, newClient.buffer.Length, SocketFlags.None, OnReceive, newClient);
            //newClient.isReceiving = true;
        }
    }

    private void OnReceive(IAsyncResult ar)
    {
        var newClient = (SocketClientConnection)ar.AsyncState;
        //try
        //{
        //    var bytesRead = newClient.socket.EndReceive(ar);
        //    if (bytesRead == 0)
        //    {
        //        EventSystem.Add(() =>
        //        {
        //            var onDisconnectedEvent = newClient.gameObject.AddComponent<OnDisconnectedFromServerEvent>();
        //            //onDisconnectedEvent.client = newClient;
        //            return onDisconnectedEvent;
        //        });
        //    }
        //    else
        //    {
        //        receivedMessage += Encoding.UTF8.GetString(newClient.buffer, 0, bytesRead);
        //        if (receivedMessage.Contains(Terminator.VALUE))
        //        {
        //            var messages = receivedMessage.Split(new[] { Terminator.VALUE }, StringSplitOptions.RemoveEmptyEntries);
        //            foreach (var message in messages)
        //            {
        //                EventSystem.Add(() =>
        //                {
        //                    var onReceiveEvent = newClient.gameObject.AddComponent<OnReceiveEvent>();
        //                    onReceiveEvent.message = message;
        //                    return onReceiveEvent;
        //                });

        //                var args = message.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
        //                if (int.TryParse(args[0], out int sessionId))
        //                {
        //                    EventSystem.Add(() =>
        //                    {
        //                        var onReceiveEvent = newClient.gameObject.AddComponent<OnReceiveFromSessionEvent>();
        //                        onReceiveEvent.sessionId = sessionId;
        //                        onReceiveEvent.message = message;
        //                        onReceiveEvent.args = args;
        //                        return onReceiveEvent;
        //                    });
        //                }
        //            }

        //            while (receivedMessage.Contains(Terminator.VALUE))
        //                receivedMessage = receivedMessage.Substring(receivedMessage.IndexOf(Terminator.VALUE) + Terminator.VALUE.Length);
        //        }
        //        newClient.socket.BeginReceive(newClient.buffer, 0, newClient.buffer.Length, SocketFlags.None, OnReceive, newClient);
        //    }
        //}
        //catch
        //{
        //    EventSystem.Add(() =>
        //    {
        //        if (!newClient || !newClient.gameObject) return null;
        //        var onDisconnectedEvent = newClient.gameObject.AddComponent<OnDisconnectedFromServerEvent>();
        //        onDisconnectedEvent.client = newClient;
        //        return onDisconnectedEvent;
        //    });
        //}
    }
}
