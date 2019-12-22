using ECSish;

public class HandleTCPClientConnectionDisconnection : MonoBehaviourSystem
{
    private void Update()
    {
        foreach(var entity in GetEntities<ServerOnCloseEvent, SocketClientConnection>())
        {
            var client = entity.Item2;
            client.gameObject.AddComponent<EntityDestroyed>();

            foreach (var sessionEntity in GetEntities<Session, EntityId>())
            {
                var session = sessionEntity.Item1;
                //if (session.connectionId == client.connectionId)
                //    session.gameObject.AddComponent<EntityDestroyed>();
            }

            foreach (var tcpServerEntity in GetEntities<TCPServer>())
                tcpServerEntity.Item1.clients.Remove(client);
        }
    }
}
