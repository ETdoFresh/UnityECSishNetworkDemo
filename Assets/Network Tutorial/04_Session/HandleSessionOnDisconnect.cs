using ECSish;

public class HandleSessionOnDisconnect : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var entity in GetEntities<OnDisconnectedEvent>())
        {
            foreach (var session in GetEntities<Session>())
                session.Item1.gameObject.AddComponent<EntityDestroyed>();
        }

        foreach (var entity in GetEntities<OnDisconnectedFromServerEvent>())
        {
            //var client = entity.Item1.client;
            //foreach (var session in GetEntities<Session>())
            //    if (session.Item1.connectionId == ((SocketClientConnection)client).connectionId)
            //        session.Item1.gameObject.AddComponent<EntityDestroyed>();
        }
    }
}
