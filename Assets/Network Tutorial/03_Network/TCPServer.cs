using ECSish;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityNetworking;

public class TCPServer : MonoBehaviourComponentData
{
    public TCPServerUnity unityNetworking;
    public string host;
    public int port;
    public bool isListening;
    public bool isAccepting;
    public Socket socket;
    public List<TCPClientConnection> clients;

    private void OnDisable()
    {
        isListening = false;
        isAccepting = false;

        foreach (var client in clients)
            try { client.socket.Dispose(); }
            catch { }

        clients.Clear();

        if (socket != null)
        {
            socket.Dispose();
            socket = null;
        }
    }
}
