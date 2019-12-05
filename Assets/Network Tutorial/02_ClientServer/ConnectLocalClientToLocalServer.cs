using ECSish;
using System.Linq;

public class ConnectLocalClientToLocalServer : MonoBehaviourSystem
{
    private void Update()
    {
        var servers = GetEntities<Server>().Select(e => e.Item1);
        var clients = GetAllEntities<Client>().Select(e => e.Item1);

        foreach (var server in servers)
        {
            var newClients = clients.Except(server.clients.Select(c => c.connection));
            foreach (var client in newClients)
            {
                var clientConnectionGameObject = server.gameObject.scene.NewGameObject();
                var clientConnection = clientConnectionGameObject.AddComponent<ClientConnection>();
                clientConnection.connection = client;
                server.clients.Add(clientConnection);
                EventSystem.Add(() =>
                {
                    var onAcceptEvent = server.gameObject.AddComponent<OnAcceptedEvent>();
                    onAcceptEvent.connection = clientConnection;
                    return onAcceptEvent;
                });

                for (int i = 0; i < server.clients.Count; i++)
                {
                    var nameTaken = server.clients.Where(c => c.name.EndsWith(i.ToString())).Count() > 0;
                    if (!nameTaken)
                    {
                        clientConnectionGameObject.name = "ClientConnection" + i.ToString();
                        return;
                    }
                }
            }

            var disconnectedClients = server.clients.Where(c => !clients.Contains(c.connection)).ToArray();
            foreach (var client in disconnectedClients)
            {
                EventSystem.Add(() =>
                {
                    var onDisconnectedEvent = server.gameObject.AddComponent<OnDisconnectedFromTCPServerEvent>();
                    onDisconnectedEvent.client = client;
                    return onDisconnectedEvent;
                });
                server.clients.Remove(client);
                Entity.Destroy(client.gameObject);
            }
        }
    }
}
