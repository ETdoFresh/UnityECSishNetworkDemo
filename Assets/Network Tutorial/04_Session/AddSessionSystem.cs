using ECSish;
using System;

public class AddSessionSystem : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var entity in GetEntities<OnReceiveEvent, Client>())
        {
            var args = entity.Item1.message.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            if (args.Length != 4) return;
            var command = args[0].ToLower();
            if (command == "addsession")
            {
                var newSessionEntity = gameObject.scene.NewGameObject();
                var session = newSessionEntity.AddComponent<Session>();
                session.id = Convert.ToInt32(args[1]);
                session.build = args[2];
                session.nickname = args[3];
                session.name = "Session" + session.id;
            }
        }

        foreach (var entity in GetEntities<OnReceiveEvent, SocketClientConnection>())
        {
            var args = entity.Item1.message.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            if (args.Length != 4) continue;
            var command = args[0].ToLower();
            if (command == "addsession")
            {
                var client = entity.Item2;
                var newSessionEntity = gameObject.scene.NewGameObject();
                var session = newSessionEntity.AddComponent<Session>();
                session.id = Convert.ToInt32(args[1]);
                session.build = args[2];
                session.nickname = args[3];
                session.name = "Session" + session.id;
                session.connection = client;
            }
        }
    }
}
