using ECSish;

public class ClearCommandSystem : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var entity in GetEntities<OnReceiveEvent, Session>())
        {
            var args = entity.Item1.Args;
            if (args.Length < 1) continue;
            var command = args[0].ToLower();
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