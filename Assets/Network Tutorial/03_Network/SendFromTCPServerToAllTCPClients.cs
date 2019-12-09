using ECSish;

public class SendFromTCPServerToAllTCPClients : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var entity in GetEntities<OnSendEvent, TCPServer>())
        {
            var message = entity.Item1.message;
            var server = entity.Item2;

            foreach (var client in server.clients)
                server.Send(client.socket, message);
        }
    }
}
