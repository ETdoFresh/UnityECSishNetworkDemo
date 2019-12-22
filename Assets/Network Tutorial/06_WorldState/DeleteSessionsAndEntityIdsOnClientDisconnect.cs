using ECSish;

public class DeleteSessionsAndEntityIdsOnClientDisconnect : MonoBehaviourSystem
{
    private void Update()
    {
        if (GetEntity<Client>() == null)
            foreach (var sessionEntity in GetEntities<Session>())
                sessionEntity.Item1.gameObject.AddComponent<EntityDestroyed>();

        foreach (var entity in GetEntities<ClientOnCloseEvent, Client>())
            foreach (var sessionEntity in GetEntities<Session>())
                sessionEntity.Item1.gameObject.AddComponent<EntityDestroyed>();

        if (GetEntity<Client>() == null)
            foreach (var networkEntity in GetEntities<EntityId>())
                networkEntity.Item1.gameObject.AddComponent<EntityDestroyed>();

        foreach (var entity in GetEntities<ClientOnCloseEvent, Client>())
            foreach (var networkEntity in GetEntities<EntityId>())
                networkEntity.Item1.gameObject.AddComponent<EntityDestroyed>();
    }
}
