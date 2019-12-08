using ECSish;
using System.Net.Sockets;

public class SocketClientConnection : MonoBehaviourComponentData
{
    public string host;
    public int port;
    public string protocol;
    public Socket socket;
}
