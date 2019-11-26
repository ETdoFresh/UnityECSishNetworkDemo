using ECSish;
using System.Net.Sockets;
using UnityEngine;
using UnityNetworking;

[RequireComponent(typeof(TCPClientUnity))]
public class TCPClientWrapper : MonoBehaviourComponentData
{
    public TCPClientUnity unityNetworking;
    public string host;
    public int port;
    public bool isConnected;
    public Socket socket;
    public byte[] buffer = new byte[16384];

    private void Awake()
    {
        unityNetworking = GetComponent<TCPClientUnity>();
    }
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
