using ECSish;

public class ShowHideCommandSystem : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var entity in GetEntities<OnSendEvent, DebugShowOnConsole>())
        {
            var args = entity.Item1.Args;
            var debug = entity.Item2;
            if (args.Length != 2) continue;
            var command = args[0].ToLower();
            var target = args[1].ToLower();
            
            if (command == "show" && target == "input")
                debug.showInput = true;
            if (command == "show" && target == "update")
                debug.showUpdate = true;
            if (command == "hide" && target == "input")
                debug.showInput = false;
            if (command == "hide" && target == "update")
                debug.showUpdate = false;
        }
    }
}