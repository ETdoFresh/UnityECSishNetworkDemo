using ECSish;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AddSessionSystem : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var entity in GetEntities<OnReceiveEvent, Client>())
        {
            var args = entity.Item1.message.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            if (args.Length != 7) return;
            var command = args[0].ToLower();
            if (command == "addsession")
            {
                var newSessionEntity = new GameObject("Session");
                SceneManager.MoveGameObjectToScene(newSessionEntity, gameObject.scene);
                var session = newSessionEntity.AddComponent<Session>();
                session.id = Convert.ToInt32(args[1]);
                session.ip = args[2];
                session.port = Convert.ToInt32(args[3]);
                session.connectionType = args[4];
                session.build = args[5];
                session.nickname = args[6];
                newSessionEntity.name += session.id;
            }
        }

        foreach (var entity in GetEntities<OnReceiveEvent, TCPClientConnection>())
        {
            var args = entity.Item1.message.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            if (args.Length != 7) continue;
            var command = args[0].ToLower();
            if (command == "addsession")
            {
                var client = entity.Item2;
                var newSessionEntity = new GameObject("Session");
                SceneManager.MoveGameObjectToScene(newSessionEntity, gameObject.scene);
                var session = newSessionEntity.AddComponent<Session>();
                session.id = Convert.ToInt32(args[1]);
                session.ip = args[2];
                session.port = Convert.ToInt32(args[3]);
                session.connectionType = args[4];
                session.build = args[5];
                session.nickname = args[6];
                session.connectionId = client.connectionId;
                newSessionEntity.name += session.id;
            }
        }
    }
}
