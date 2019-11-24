using ECSish;
using System;
using System.Net.Sockets;
using System.Text;

public class SendFromTCPServerToAllTCPClients : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var entity in GetEntities<OnSendEvent, TCPServer>())
        {
            var message = entity.Item1.message;
            message += Terminator.VALUE;
            var bytes = Encoding.UTF8.GetBytes(message);
            var server = entity.Item2;
            foreach (var client in server.clients)
                if (client.socket != null && client.socket.Connected)
                    client.socket.BeginSend(bytes, 0, bytes.Length, SocketFlags.None, OnSent, client);
        }
    }

    private void OnSent(IAsyncResult ar)
    {
        var client = (TCPClientConnection)ar.AsyncState;
        try { client.socket.EndSend(ar); }
        catch { }
    }
}
