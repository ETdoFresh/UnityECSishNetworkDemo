using ECSish;
using System;

public class ClearCommandSystem : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var entity in GetEntities<OnReceiveEvent, SocketClientConnection>())
        {
            var args = entity.Item1.message.Split(new[] { " " }, StringSplitOptions.None);
            if (args.Length < 2) continue;
            var command = args[1].ToLower();
            var client = entity.Item2;
            if (command == "clear")
            {
                foreach (var toBeDestroyed in GetEntities<EntityId>())
                        toBeDestroyed.Item1.gameObject.AddComponent<EntityDestroyed>();

                var message = $"All Network Objects have been destroyed.";
            }
        }
    }
}