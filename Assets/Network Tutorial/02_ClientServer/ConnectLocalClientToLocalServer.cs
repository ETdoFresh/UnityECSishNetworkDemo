using ECSish;

public class ConnectLocalClientToLocalServer : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var clientEntity in GetEntities<LocalClient>())
        {
            var client = clientEntity.Item1;
            foreach (var serverEntity in GetEntities<LocalServer>())
            {
                var server = serverEntity.Item1;
                if (!server.clients.Contains(client))
                {
                    server.clients.Add(client);
                    client.server = server;
                    EventSystem.Add(() =>
                    {
                        var onAcceptEvent = server.gameObject.AddComponent<OnLocalClientAccepted>();
                        onAcceptEvent.client = client;
                        return onAcceptEvent;
                    });
                }
            }
        }
    }
}
