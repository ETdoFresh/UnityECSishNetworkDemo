using ECSish;
using System.Net.Sockets;
using UnityNetworking;

public class TCPClient : MonoBehaviourComponentData
{
    public TCPClientUnity unityNetworking = new TCPClientUnity();
    public string host;
    public int port;
    public bool isConnected;
    public bool isReceiving;
    public Socket socket;
    public byte[] buffer = new byte[16384];

    private void OnDisable()
    {
        if (socket != null)
        {
            if (socket.Connected) socket.Disconnect(false);
            socket.Dispose();
            socket = null;
        }
    }
}
