using ECSish;
using System;
using System.Net.Sockets;
using System.Text;

public class StartTCPClientConnectionReceiveOnAccepted : MonoBehaviourSystem
{
    public string receivedMessage;

    private void Update()
    {
        foreach (var entity in GetEntities<OnAcceptedEvent, TCPServer>())
        {
            var newClient = entity.Item1.connection;
            newClient.socket.BeginReceive(newClient.buffer, 0, newClient.buffer.Length, SocketFlags.None, OnReceive, newClient);
            newClient.isReceiving = true;
        }
    }

    private void OnReceive(IAsyncResult ar)
    {
        var newClient = (TCPClientConnection)ar.AsyncState;
        try
        {
            var bytesRead = newClient.socket.EndReceive(ar);
            if (bytesRead == 0)
            {
                EventSystem.Add(() =>
                {
                    var onDisconnectedEvent = newClient.gameObject.AddComponent<OnDisconnectedFromTCPServerEvent>();
                    onDisconnectedEvent.client = newClient;
                    return onDisconnectedEvent;
                });
            }
            else
            {
                receivedMessage += Encoding.UTF8.GetString(newClient.buffer, 0, bytesRead);
                if (receivedMessage.Contains(Terminator.VALUE))
                {
                    var messages = receivedMessage.Split(new[] { Terminator.VALUE }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var message in messages)
                        EventSystem.Add(() =>
                        {
                            var onReceiveEvent = newClient.gameObject.AddComponent<OnReceiveEvent>();
                            onReceiveEvent.message = message;
                            return onReceiveEvent;
                        });

                    while (receivedMessage.Contains(Terminator.VALUE))
                        receivedMessage = receivedMessage.Substring(receivedMessage.IndexOf(Terminator.VALUE) + Terminator.VALUE.Length);
                }
                newClient.socket.BeginReceive(newClient.buffer, 0, newClient.buffer.Length, SocketFlags.None, OnReceive, newClient);
            }
        }
        catch
        {
            EventSystem.Add(() =>
            {
                var onDisconnectedEvent = newClient.gameObject.AddComponent<OnDisconnectedFromTCPServerEvent>();
                onDisconnectedEvent.client = newClient;
                return onDisconnectedEvent;
            });
        }
    }
}
