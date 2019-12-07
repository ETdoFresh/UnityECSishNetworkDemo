using ECSish;
using System;

public class HelpCommandSystem : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var entity in GetEntities<OnReceiveFromSessionEvent, SocketClientConnection>())
        {
            var args = entity.Item1.args;
            if (args.Length != 2) continue;

            var command = args[1].ToLower();
            var server = entity.Item2;
            if (command == "help" || command == "?")

                EventUtility.CreateOnSendEvent(server.gameObject,
$@"{entity.Item1.sessionId} ----------
Commands:
----------
Connect {{IP{{:PORT}}}}
Disconnect
Exit/Quit
Restart
Test
Chat {{ChatMessage}}
Whisper {{ClientIndex}} {{ChatMessage}}
Help/?");

        }
    }
}