using ECSish;
using System;
using System.Linq;

public class StartWebSocketClientReceiveOnConnectedWithSessionSupport : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var entity in GetEntities<OnConnectedEvent, WebSocketClient>())
        {
            var client = entity.Item2;
            if (!client) continue;
            if (!client.isConnected) continue;

#if !UNITY_WEBGL || UNITY_EDITOR
            client.stream.BeginRead(client.buffer, 0, client.buffer.Length, OnReceive, client);
#endif
            client.isReceiving = true;
        }
    }

    private void OnReceive(IAsyncResult ar)
    {
        var client = (WebSocketClient)ar.AsyncState;

        try
        {
            int bytesRead = client.stream.EndRead(ar);
            client.received.AddRange(client.buffer.Take(bytesRead));

            if (bytesRead > 0)
            {
                while (client.received.Count >= WebSocket.PacketLength(client.received))
                {
                    var receivedMessage = WebSocket.BytesToString(client.received.ToArray());
                    var messages = receivedMessage.Split(new[] { Terminator.VALUE }, StringSplitOptions.RemoveEmptyEntries);
                    client.received.RemoveRange(0, (int)WebSocket.PacketLength(client.received));

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
                }
                client.stream.BeginRead(client.buffer, 0, client.buffer.Length, OnReceive, client);
            }
            else
            {
                EventSystem.Add(() => client.gameObject.AddComponent<OnDisconnectedEvent>());
            }
        }
        catch (Exception e)
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
