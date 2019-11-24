using ECSish;

public class HandleOnDisconnectClientEvent : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var entity in GetEntities<OnDisconnectedEvent, TCPClient>())
        {
            var client = entity.Item2;
            client.isReceiving = false;
            client.isConnected = false;
            if (client.socket != null && client.socket.Connected)
                client.socket.Close();
            client.socket = null;
        }
    }
}
