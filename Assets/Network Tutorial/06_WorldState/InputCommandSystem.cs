using ECSish;
using System;

public class InputCommandSystem : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var entity in GetEntities<OnReceiveEvent, Session>())
        {
            var args = entity.Item1.Args;
            if (args.Length != 5) continue;

            var command = args[1].ToLower();
            if (command == "input")
            {
                var session = entity.Item2;
                var horizontal = Convert.ToInt32(args[2]);
                var vertical = Convert.ToInt32(args[3]);
                var jumpPressed = Convert.ToBoolean(args[4]);

                foreach (var tcpInputEntity in GetEntities<SessionInput>())
                    if (tcpInputEntity.Item1.sessionId == session.id)
                    {
                        tcpInputEntity.Item1.horizontal = horizontal;
                        tcpInputEntity.Item1.vertical = vertical;
                        tcpInputEntity.Item1.jumpPressed = jumpPressed;
                    }
            }
        }
    }
}
