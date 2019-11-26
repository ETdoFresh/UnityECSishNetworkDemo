using ECSish;
using System;

public class ParseOnReceiveFromSessionEvent : MonoBehaviourSystem
{
    public string receivedMessage;

    private void Update()
    {
        foreach (var entity in GetEntities<OnReceiveEvent>())
        {
            var gameObject = entity.Item1.gameObject;
            var message = entity.Item1.message;
            var args = message.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            if (int.TryParse(args[0], out int sessionId))
            {
                EventSystem.Add(() =>
                {
                    var onReceiveEvent = gameObject.AddComponent<OnReceiveFromSessionEvent>();
                    onReceiveEvent.sessionId = sessionId;
                    onReceiveEvent.message = message;
                    onReceiveEvent.args = args;
                    return onReceiveEvent;
                });
            }
        }
    }
}