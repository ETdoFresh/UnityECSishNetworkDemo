using ECSish;
using System;
using System.Linq;

public class ParseOnReceiveFromSessionEvent : MonoBehaviourSystem
{
    public string receivedMessage;

    private void Update()
    {
        var entities = GetEntities<OnReceiveEvent>();
        if (entities.Count() == 0) return;

        var sessions = GetEntities<Session>();
        foreach (var entity in entities)
        {
            var gameObject = entity.Item1.gameObject;
            var message = entity.Item1.message;
            var args = message.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            if (int.TryParse(args[0], out int sessionId))
            {
                var session = sessions.Where(s => s.Item1.id == sessionId).FirstOrDefault();
                if (session != null)
                {
                    ECSEvent.Add(() =>
                    {
                        var onReceiveEvent = session.Item1.gameObject.AddComponent<OnReceiveEvent>();
                        onReceiveEvent.message = message;
                        return onReceiveEvent;
                    });
                }
            }
        }
    }
}