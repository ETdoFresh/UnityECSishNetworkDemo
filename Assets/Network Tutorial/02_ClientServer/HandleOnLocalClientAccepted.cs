using ECSish;

public class HandleOnLocalClientAccepted : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var entity in GetEntities<OnLocalClientAccepted, LocalServer>())
        {
            var client = entity.Item1.client;
            var server = entity.Item2;

            server.clients.Add(client);
            client.server = server;
        }
    }
}