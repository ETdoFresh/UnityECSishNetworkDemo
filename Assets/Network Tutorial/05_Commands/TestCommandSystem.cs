using ECSish;
using System;

public class TestCommandSystem : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var entity in GetEntities<OnReceiveEvent, Session>())
        {
            var args = entity.Item1.message.Split(new[] { " " }, StringSplitOptions.None);
            if (args.Length < 2) continue;
            var session = entity.Item2;
            var command = args[1].ToLower();
            if (command == "test")
                ECSEvent.Create<OnSendEvent>(session, "Server reply to Test Command");
        }
    }
}
