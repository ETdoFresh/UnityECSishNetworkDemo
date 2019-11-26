using ECSish;
using UnityNetworking;

public class SendFromClientOnSendEventSystem : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var entity in GetEntities<OnSendEvent, Client>())
        {
            var tcpClient = entity.Item1.GetComponent<TCPClientUnity>();
            if (tcpClient) tcpClient.Send(entity.Item1.message);

            var webSocketClient = entity.Item1.GetComponent<WebSocketClientUnity>();
            if (webSocketClient) webSocketClient.Send(entity.Item1.message);
        }
    }
}
