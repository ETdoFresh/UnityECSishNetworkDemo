using ECSish;

public class ConnectCommandSystem : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var entity in GetEntities<OnSendEvent, Client>())
        {
            var args = entity.Item1.Args;
            if (args.Length < 1) continue;
            var command = args[0].ToLower();
            if (command == "connect")
            {
                var tcpClient = entity.Item2.GetComponent<TCPClient>();
                if (tcpClient)
                    tcpClient.Connect();

                var webSocketClient = entity.Item2.GetComponent<WebSocketClient>();
                if (webSocketClient)
                    webSocketClient.Connect();
            }
        }
    }
}
