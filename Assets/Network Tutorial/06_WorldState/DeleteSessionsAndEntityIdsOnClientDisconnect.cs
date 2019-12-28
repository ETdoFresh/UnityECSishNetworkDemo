using ECSish;

public class DeleteSessionsAndEntityIdsOnClientDisconnect : MonoBehaviourSystem
{
    private void Update()
    {
        if (IsClientMissingOrIsClosed())
        {
            foreach (var entity in GetEntities<Session>())
                entity.Item1.gameObject.AddComponent<EntityDestroyed>();

            foreach (var entity in GetEntities<EntityId>())
                entity.Item1.gameObject.AddComponent<EntityDestroyed>();
        }
    }

    private bool IsClientMissingOrIsClosed()
    {
        return GetEntity<Client>() == null
            || GetEntity<Client, ClientOnCloseEvent>() != null;
    }
}
