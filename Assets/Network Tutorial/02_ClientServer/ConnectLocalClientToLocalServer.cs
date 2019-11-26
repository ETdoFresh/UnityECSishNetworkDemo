using ECSish;
using System.Linq;

public class ConnectLocalClientToLocalServer : MonoBehaviourSystem
{
    private void Update()
    {
        var servers = GetEntities<Server>().Select(e => e.Item1);
        var clients = GetEntities<Client>().Select(e => e.Item1);
        var clientConnections = GetEntities<ClientConnection>().Select(e => e.Item1);

        foreach (var client in clients)
        {
            if (clientConnections.Select(cc => (Client)cc.connection == client).Count() == 0)
            {
                foreach (var server in servers)
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
                }
            }
        }
    }
}
