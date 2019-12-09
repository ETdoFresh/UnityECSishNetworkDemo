using ECSish;

public class SendFromTCPServerToTCPClientConnection : MonoBehaviourSystem
{
    private void Update()
    {
        var server = GetEntity<TCPServer>();
        if (server == null) return;

        foreach (var entity in GetEntities<OnSendEvent, SocketClientConnection>())
        {
            var message = entity.Item1.message;
            var client = entity.Item2.socket;
            server.Item1.Send(client, message);
        }
    }
}
