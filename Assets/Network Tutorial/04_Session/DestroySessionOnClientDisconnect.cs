using ECSish;

public class DestroySessionOnClientDisconnect : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var entity in GetEntities<ClientOnCloseEvent>())
        {
            foreach (var session in GetEntities<Session>())
                session.Item1.gameObject.AddComponent<EntityDestroyed>();
        }
    }
}
