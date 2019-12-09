using ECSish;

public class DestroySessionOnClientDisconnect : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var entity in GetEntities<OnDisconnectedEvent>())
        {
            foreach (var session in GetEntities<Session>())
                session.Item1.gameObject.AddComponent<EntityDestroyed>();
        }
    }
}
