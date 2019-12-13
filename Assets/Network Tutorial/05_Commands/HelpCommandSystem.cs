using ECSish;

public class HelpCommandSystem : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var entity in GetEntities<OnReceiveEvent, Session>())
        {
            var args = entity.Item1.Args;
            if (args.Length != 2) continue;

            var command = args[1].ToLower();
            var server = entity.Item2;
            if (command == "help" || command == "?")

                ECSEvent.Create<OnSendEvent>(server,
@"----------
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