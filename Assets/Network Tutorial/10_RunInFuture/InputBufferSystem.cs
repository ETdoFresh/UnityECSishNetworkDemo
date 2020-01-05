using ECSish;
using System;
using System.Linq;

public class InputBufferSystem : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var entity in GetEntities<OnReceiveEvent, Session>())
        {
            var onReceiveEvent = entity.Item1;
            var session = entity.Item2;
            var inputBuffer = session.GetComponent<InputBuffer>();
            if (!inputBuffer) inputBuffer = session.gameObject.AddComponent<InputBuffer>();

            var args = onReceiveEvent.Args;
            if (args.Length < 1) continue;

            var command = args[0].ToLower();
            var i = 1;
            if (command == "input")
            {
                while (i < args.Length)
                {
                    var tick = Convert.ToInt32(args[i++]);
                    var horizontal = Convert.ToInt32(args[i++]);
                    var vertical = Convert.ToInt32(args[i++]);
                    var jumpPress = Convert.ToBoolean(args[i++]);
                    inputBuffer.AddNewInput(tick, horizontal, vertical, jumpPress);
                }
            }
        }

        foreach(var serverTickEntity in GetEntities<ServerTick>())
            foreach(var entity in GetEntities<Session, InputBuffer>())
            {
                var serverTick = serverTickEntity.Item1.tick;
                var session = entity.Item1;
                var inputBuffer = entity.Item2;

                var input = inputBuffer.inputs.Where(i => i.tick == serverTick).FirstOrDefault();
                if (input != null)
                    foreach(var sessionInputEntity in GetEntities<SessionInput>())
                    {
                        var sessionInput = sessionInputEntity.Item1;
                        if (sessionInput.sessionId == session.id)
                        {
                            sessionInput.horizontal = input.horizontal;
                            sessionInput.vertical = input.vertical;
                            sessionInput.jumpPressed = input.jumpPressed;
                        }
                    }

                inputBuffer.RemoveTimesOnOrBeforeTick(serverTick);
            }
    }
}
