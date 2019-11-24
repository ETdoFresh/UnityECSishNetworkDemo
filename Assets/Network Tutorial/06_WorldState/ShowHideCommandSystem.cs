using ECSish;

public class ShowHideCommandSystem : MonoBehaviourSystem
{
    private void Update()
    {
        foreach(var entity in GetEntities<OnSendEvent, Client>())
        {
            var args = entity.Item1.message.Split(' ');
            if (args.Length != 3) continue;
            var command = args[1].ToLower();
            var target = args[2].ToLower();
            if (command == "show" && target == "input")
                foreach (var showInput in gameObject.scene.FindObjectsOfType<ShowInputOnConsole>(true))
                    showInput.enabled = true;

            if (command == "show" && target == "update")
                foreach (var showInput in gameObject.scene.FindObjectsOfType<ShowUpdateOnConsole>(true))
                    showInput.enabled = true;

            if (command == "hide" && target == "input")
                foreach (var showInput in gameObject.scene.FindObjectsOfType<ShowInputOnConsole>(true))
                    showInput.enabled = false;

            if (command == "hide" && target == "update")
                foreach (var showInput in gameObject.scene.FindObjectsOfType<ShowUpdateOnConsole>(true))
                    showInput.enabled = false;
        }
    }
}