using ECSish;

public class HandleTCPClientConnectionDisconnection : MonoBehaviourSystem
{
    private void Update()
    {
        foreach(var entity in GetEntities<ServerOnCloseEvent, SocketClientConnection>())
        {
            var client = entity.Item2;
            client.gameObject.AddComponent<EntityDestroyed>();

            foreach (var tcpServerEntity in GetEntities<TCPServer>())
                tcpServerEntity.Item1.clients.Remove(client);
        }
    }
}
