using ECSish;
using System;
using System.Linq;

public class ParseOnReceiveFromSessionEvent : MonoBehaviourSystem
{
    private void Update()
    {
        var sessions = GetEntities<Session>();
        var onReceiveEvents = GetEntities<OnReceiveEvent, SocketClientConnection>().Select(e => e.Item1);
        if (onReceiveEvents.Count() == 0)
            onReceiveEvents = GetEntities<OnReceiveEvent, Client>().Select(e => e.Item1);

        foreach (var onReceiveEvent in onReceiveEvents)
        {
            var gameObject = onReceiveEvent.gameObject;
            var message = onReceiveEvent.message;
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