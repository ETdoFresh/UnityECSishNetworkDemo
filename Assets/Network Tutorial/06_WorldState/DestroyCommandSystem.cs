using ECSish;
using System;

public class DestroyCommandSystem : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var entity in GetEntities<OnReceiveEvent, Session>())
        {
            var args = entity.Item1.Args;
            if (args.Length < 2) continue;
            var command = args[0].ToLower();
            var client = entity.Item2;
            if (command == "destroy")
            {
                var entityId = Convert.ToInt32(args[1]);

                foreach (var toBeDestroyed in GetEntities<EntityId>())
                    if (toBeDestroyed.Item1.entityId == entityId)
                        toBeDestroyed.Item1.gameObject.AddComponent<EntityDestroyed>();

                var message = $"Entity Id {entityId} has been destroyed.";
                ECSEvent.Create<OnSendEvent>(client.gameObject, message);
            }
        }
    }
}
