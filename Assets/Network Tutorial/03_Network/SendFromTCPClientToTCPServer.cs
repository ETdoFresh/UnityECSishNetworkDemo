﻿using ECSish;
using System;
using System.Net.Sockets;
using System.Text;

public class SendFromTCPClientToTCPServer : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var entity in GetEntities<OnSendEvent, TCPClient>())
        {
            var client = entity.Item2;
            if (!client) continue;
            if (client.socket == null) continue;
            if (!client.socket.Connected) continue;

            var message = entity.Item1.message;
            message += Terminator.VALUE;
            var bytes = Encoding.UTF8.GetBytes(message);
            client.socket.BeginSend(bytes, 0, bytes.Length, SocketFlags.None, OnSent, client);
        }
    }

    private void OnSent(IAsyncResult ar)
    {
        var client = (TCPClient)ar.AsyncState;
        client.socket.EndSend(ar);
    }
}