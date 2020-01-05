using ECSish;
using System;
using UnityEngine;

public class InputWithTickCommandSystem : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var entity in GetEntities<OnReceiveEvent, Session>())
        {
            var args = entity.Item1.Args;
            if (args.Length < 1) continue;

            var command = args[0].ToLower();
            if (command == "input")
            {
                var session = entity.Item2;

                var tick = Convert.ToInt32(args[1]);
                foreach (var clientTick in GetEntities<ClientTick>())
                {
                    clientTick.Item1.lastReceivedTick = tick;
                    clientTick.Item1.lastReceivedTime = Time.time;
                }

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
