using ECSish;
using System.Linq;
using System.Net;
using System.Net.Sockets;

public class StartTCPServerListenOnEnable : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var entity in GetEntities<OnEnableEvent, TCPServer>())
        {
            var server = entity.Item2;
            if (server.isListening) continue;

            IPEndPoint localEndPoint = null;
            if (server.host != "")
            {
                var ipHostInfo = Dns.GetHostEntry(server.host);
                var ipAddress = ipHostInfo.AddressList.Where(ip => ip.AddressFamily == AddressFamily.InterNetwork).FirstOrDefault();
                localEndPoint = new IPEndPoint(ipAddress, server.port);
            }
            else
            {
                localEndPoint = new IPEndPoint(IPAddress.Any, server.port);
            }
            server.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            server.socket.Bind(localEndPoint);
            server.socket.Listen(100);
            server.isListening = true;

            EventSystem.Add(() => server.gameObject.AddComponent<OnListeningEvent>());
        }
    }
}
