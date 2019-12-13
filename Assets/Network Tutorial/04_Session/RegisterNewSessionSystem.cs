using ECSish;
using UnityEngine;

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

                var newSessionEntity = gameObject.scene.NewGameObject();
                var session = newSessionEntity.AddComponent<Session>();
                session.id = Session.GetNextId();
                session.build = args[1];
                session.nickname = "Guest" + session.id;
                session.connection = client;
                session.lastReceived = Time.time;
                session.name = "Session" + session.id;

                var message = $"AddSession {session.id} {session.build} {session.nickname}";
                ECSEvent.Create<OnSendEvent>(client.gameObject, message);
            }
        }
    }
}
