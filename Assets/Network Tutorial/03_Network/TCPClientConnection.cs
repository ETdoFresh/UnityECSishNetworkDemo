using ECSish;
using System.Net.Sockets;

public class TCPClientConnection : MonoBehaviourComponentData
{
    public static int nextId;

    public int connectionId = -1;
    public string host;
    public int port;
    public bool isReceiving;
    public Socket socket;
    public byte[] buffer = new byte[2048];
}
