using ECSish;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RegisterNewSessionSystem : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var entity in GetEntities<OnReceiveEvent, SocketClientConnection>())
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
                session.id = Session.GetNextId();
                session.build = args[1];
                session.nickname = "Guest" + session.id;
                session.connection = client;
                newSessionEntity.name += session.id;

                var message = $"AddSession {session.id} {session.build} {session.nickname}";
                message += "\r\n";
                var bytes = Encoding.UTF8.GetBytes(message);
                client.socket.BeginSend(bytes, 0, bytes.Length, SocketFlags.None, OnSent, client);
            }
        }
    }

    private void OnSent(IAsyncResult ar)
    {
        var client = (SocketClientConnection)ar.AsyncState;
        client.socket.EndSend(ar);
    }
}
