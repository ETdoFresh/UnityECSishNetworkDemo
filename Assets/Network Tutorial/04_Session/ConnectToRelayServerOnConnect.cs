using ECSish;
using System;
using System.Net.Sockets;
using System.Text;
using Random = UnityEngine.Random;

public class ConnectToRelayServerOnConnect : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var entity in GetEntities<OnConnectedEvent, TCPClient, RelayClient>())
        {
            var client = entity.Item2;
            if (!client) continue;
            if (client.socket == null) continue;
            if (!client.socket.Connected) continue;

            var message = "StartSession";
            message += " Guest" + Random.Range(10000, 100000);

#if UNITY_EDITOR
            message += " Editor";
#elif UNITY_WEBGL
            message += " WebGL";
#elif UNITY_STANDALONE_WIN
            message += " Windows";
#elif UNITY_STANDALONE_OSX
            message += " OSX";
#elif UNITY_STANDALONE_LINUX
            message += " Linux";
#endif

            message += Terminator.VALUE;
            var bytes = Encoding.UTF8.GetBytes(message);
            client.socket.BeginSend(bytes, 0, bytes.Length, SocketFlags.None, OnSent, client);
        }

        foreach (var entity in GetEntities<OnReceiveEvent, TCPClient, RelayClient>())
        {
            var client = entity.Item2;
            if (!client) continue;
            if (client.socket == null) continue;
            if (!client.socket.Connected) continue;

            var receivedMessage = entity.Item1.message;
            if (!receivedMessage.ToLower().Contains("addsession")) continue;

            entity.Item3.assignedSession = true;

            var lobby = entity.Item3.lobby;
            var bytes = Encoding.UTF8.GetBytes($"JoinLobby {lobby}{Terminator.VALUE}");
            client.socket.BeginSend(bytes, 0, bytes.Length, SocketFlags.None, OnSent, client);
        }

        foreach (var entity in GetEntities<OnReceiveEvent, TCPClient, RelayClient>())
        {
            var client = entity.Item2;
            if (!client) continue;
            if (client.socket == null) continue;
            if (!client.socket.Connected) continue;

            var receivedMessage = entity.Item1.message;
            if (!receivedMessage.ToLower().Contains("success") || !receivedMessage.ToLower().Contains("lobby")) continue;

            entity.Item3.joinedLobby = true;

            var room = entity.Item3.room;
            var bytes = Encoding.UTF8.GetBytes($"JoinRoom {room}{Terminator.VALUE}");
            client.socket.BeginSend(bytes, 0, bytes.Length, SocketFlags.None, OnSent, client);
        }

        foreach (var entity in GetEntities<OnReceiveEvent, TCPClient, RelayClient>())
        {
            var client = entity.Item2;
            if (!client) continue;
            if (client.socket == null) continue;
            if (!client.socket.Connected) continue;

            var receivedMessage = entity.Item1.message;
            if (!receivedMessage.ToLower().Contains("success") || !receivedMessage.ToLower().Contains("room")) continue;

            entity.Item3.joinedRoom = true;
        }
    }

    private void OnSent(IAsyncResult ar)
    {
        var client = (TCPClient)ar.AsyncState;
        client.socket.EndSend(ar);
    }
}