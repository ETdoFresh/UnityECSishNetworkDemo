using ECSish;
using System.Runtime.InteropServices;

public class HandleOnDisconnectWebSocketClientEvent : MonoBehaviourSystem
{
    [DllImport("__Internal")] private static extern void SocketClose(int websocket);

    private void Update()
    {
        foreach (var entity in GetEntities<OnDisconnectedEvent, WebSocketClient>())
        {
            var client = entity.Item2;
            client.isReceiving = false;
            client.hasPerformedHandshake = false;
            client.isConnected = false;

#if UNITY_WEBGL && !UNITY_EDITOR
            SocketClose(client.websocketId);
            client.readyState = WebSocketClient.ReadyState.Closed;
#else
            if (client.socket != null && client.socket.Connected)
                client.socket.Close();
            client.socket = null;
#endif
        }
    }
}