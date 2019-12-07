using ECSish;
using System;

public class RemoveSessionSystem : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var entity in GetEntities<OnSendEvent, Client>())
        {
            var args = entity.Item1.message.Split(' ');
            if (args.Length != 2) continue;

            var command = args[1].ToLower();
            if (command == "removesession")
            {
                var sessionId = Convert.ToInt32(args[0]);
                foreach (var sessionEntity in GetEntities<Session>())
                    if (sessionEntity.Item1.id == sessionId)
                        sessionEntity.Item1.gameObject.AddComponent<EntityDestroyed>();
            }
        }

        foreach (var entity in GetEntities<OnReceiveFromSessionEvent, SocketClientConnection>())
        {
            var args = entity.Item1.args;
            if (args.Length != 2) continue;

            var command = args[1].ToLower();
            if (command == "removesession")
            {
                foreach (var sessionEntity in GetEntities<Session>())
                    if (sessionEntity.Item1.id == entity.Item1.sessionId)
                        sessionEntity.Item1.gameObject.AddComponent<EntityDestroyed>();
            }
        }
    }
}