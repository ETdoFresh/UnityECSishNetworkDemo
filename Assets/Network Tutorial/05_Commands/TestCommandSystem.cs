using ECSish;
using System;
using System.Linq;

public class TestCommandSystem : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var entity in GetEntities<OnReceiveFromSessionEvent, TCPClientConnection>())
        {
            var args = entity.Item1.message.Split(new[] { " " }, StringSplitOptions.None);
            if (args.Length < 2) continue;
            var sessionId = entity.Item1.sessionId;
            var command = args[1].ToLower();
            var client = entity.Item2;
            if (command == "test")
                EventUtility.CreateOnSendEvent(client.gameObject, $"{sessionId} Server reply to Test Command");
        }
    }
}
