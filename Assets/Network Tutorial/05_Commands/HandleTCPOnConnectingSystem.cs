using ECSish;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;

public class HandleTCPOnConnectingSystem : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var entity in GetEntities<OnConnectingEvent, TCPClient>())
        {
                var client = entity.Item2;
                if (client.socket != null) continue;
                if (client.isConnected) continue;

                var ipHostInfo = Dns.GetHostEntry(client.host);
                var ipAddress = ipHostInfo.AddressList.Where(ip => ip.AddressFamily == AddressFamily.InterNetwork).FirstOrDefault();
                var remoteEndPoint = new IPEndPoint(ipAddress, client.port);
                client.socket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                client.socket.BeginConnect(remoteEndPoint, OnConnected, client);
        }
    }

    private void OnConnected(IAsyncResult ar)
    {
        var client = (TCPClient)ar.AsyncState;
        if (client.socket.Connected)
        {
            client.isConnected = true;
            EventSystem.Add(() => client.gameObject.AddComponent<OnConnectedEvent>());
        }
        else
        {
            client.socket.Dispose();
            client.socket = null;
            EventSystem.Add(() => client.gameObject.AddComponent<OnCouldNotConnectEvent>());
        }
    }
}
