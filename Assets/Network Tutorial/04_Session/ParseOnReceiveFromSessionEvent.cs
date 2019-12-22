using ECSish;
using System;
using System.Linq;

public class ParseOnReceiveFromSessionEvent : MonoBehaviourSystem
{
    private void Update()
    {
        var entities = GetEntities<OnReceiveEvent, SocketClientConnection>();
        if (entities.Count() == 0) return;

        var sessions = GetEntities<Session>();
        foreach (var entity in entities)
        {
            var gameObject = entity.Item1.gameObject;
            var message = entity.Item1.message;
            var args = message.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            if (int.TryParse(args[0], out int sessionId))
            {
                message = message.Substring(message.IndexOf(args[0]) + args[0].Length + 1);
                var session = sessions.Where(s => s.Item1.id == sessionId).FirstOrDefault();
                if (session != null)
                {
                    ECSEvent.Create<OnReceiveEvent>(session.Item1, message);
                }
            }
        }
    }
}