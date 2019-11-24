using ECSish;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;

public class WebSocketClient : MonoBehaviourComponentData
{
    public string url;
    public bool isConnected;
    public bool hasPerformedHandshake;
    public bool isReceiving;
    public Socket socket;
    public Stream stream;
    public byte[] buffer = new byte[2048];
    public List<byte> received = new List<byte>();

    public enum ReadyState { Connecting, Open, Closing, Closed };
    public ReadyState readyState = ReadyState.Closed;
    public int websocketId = -1;

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

