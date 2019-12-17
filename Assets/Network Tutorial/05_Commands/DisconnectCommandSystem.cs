using ECSish;

public class DisconnectCommandSystem : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var entity in GetEntities<OnSendEvent, Client>())
        {
            var args = entity.Item1.Args;
            if (args.Length == 0) continue;
            var command = args[0].ToLower();
            if (command == "disconnect")
            {
                var tcpClient = entity.Item2.GetComponent<TCPClient>();
                if (tcpClient)
                    tcpClient.Disconnect();

                var webSocketClient = entity.Item2.GetComponent<WebSocketClient>();
                if (webSocketClient)
                    webSocketClient.Disconnect();
            }
        }
    }
}
