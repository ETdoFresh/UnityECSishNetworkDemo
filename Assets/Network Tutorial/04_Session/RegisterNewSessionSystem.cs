using ECSish;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class RegisterNewSessionSystem : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var entity in GetEntities<OnReceiveEvent, TCPClientConnection>())
        {
            var args = entity.Item1.message.Split(' ');
            if (args.Length != 2) continue;

            var command = args[0].ToLower();
            if (command == "registernewsession")
            {
                var client = entity.Item2;

                var newSessionEntity = new GameObject("Session");
                SceneManager.MoveGameObjectToScene(newSessionEntity, gameObject.scene);
                var session = newSessionEntity.AddComponent<Session>();
                session.id = Session.nextId++;
                session.ip = ((IPEndPoint)client.socket.RemoteEndPoint).Address.ToString();
                session.port = ((IPEndPoint)client.socket.RemoteEndPoint).Port;
                session.connectionType = client.socket.ProtocolType.ToString().ToUpper();
                session.build = args[1];
                session.nickname = "Guest" + Random.Range(10000, 100000);
                session.connectionId = client.connectionId;
                newSessionEntity.name += session.id;

                var message = $"AddSession {session.id} {session.ip} {session.port} {session.connectionType} {session.build} {session.nickname}";
                message += Terminator.VALUE;
                var bytes = Encoding.UTF8.GetBytes(message);
                client.socket.BeginSend(bytes, 0, bytes.Length, SocketFlags.None, OnSent, client);
            }
        }
    }

    private void OnSent(IAsyncResult ar)
    {
        var client = (TCPClientConnection)ar.AsyncState;
        client.socket.EndSend(ar);
    }
}
