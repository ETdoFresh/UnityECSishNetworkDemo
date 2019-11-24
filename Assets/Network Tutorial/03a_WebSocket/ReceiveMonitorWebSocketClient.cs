using ECSish;
using System.Runtime.InteropServices;
using System.Text;

public class ReceiveMonitorWebSocketClient : MonoBehaviourSystem
{
    [DllImport("__Internal")] private static extern int SocketState(int websocket);
    [DllImport("__Internal")] private static extern int SocketRecvLength(int websocket);
    [DllImport("__Internal")] private static extern void SocketRecv(int websocket, byte[] ptr, int length);

    private void Update()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        foreach (var entity in GetEntities<WebSocketClient>())
        {
            var client = entity.Item1;
            if (client.websocketId == -1) continue;

            //if (SocketState(client.websocketId) == (int)WebSocketClient.ReadyState.Closing)
            //{
            //    EventSystem.Add(() => client.gameObject.AddComponent<OnDisconnectedEvent>());
            //    return;
            //}

            if (client.readyState == WebSocketClient.ReadyState.Closed || client.readyState == WebSocketClient.ReadyState.Connecting)
                if (SocketState(client.websocketId) == (int)WebSocketClient.ReadyState.Open)
                {
                    EventSystem.Add(() => client.gameObject.AddComponent<OnConnectedEvent>());
                    client.isConnected = true;
                    client.hasPerformedHandshake = true;
                    client.isReceiving = true;
                    client.readyState = WebSocketClient.ReadyState.Open;
                }

            if (SocketState(client.websocketId) == (int)WebSocketClient.ReadyState.Open)
            {
                var received = Receive(client);
                if (received != null)
                {
                    var message = Encoding.UTF8.GetString(received, 0, received.Length);
                    var eofIndex = message.IndexOf(Terminator.VALUE);
                    if (eofIndex >= 0)
                        message = message.Substring(0, eofIndex);
                    EventSystem.Add(() =>
                    {
                        var onReceiveEevent = client.gameObject.AddComponent<OnReceiveEvent>();
                        onReceiveEevent.message = message;
                        return onReceiveEevent;
                    });
                }
            }
        }
#endif
    }

    public byte[] Receive(WebSocketClient client)
    {
        var length = SocketRecvLength(client.websocketId);
        if (length == 0) return null;
        var bytes = new byte[length];
        SocketRecv(client.websocketId, bytes, bytes.Length);
        return bytes;
    }
}
