using ECSish;
using System;

public class DestroyCommandSystem : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var entity in GetEntities<OnReceiveEvent, TCPClientConnection>())
        {
            var args = entity.Item1.message.Split(new[] { " " }, StringSplitOptions.None);
            if (args.Length < 2) continue;
            var command = args[1].ToLower();
            var client = entity.Item2;
            if (command == "destroy")
            {
                if (args.Length < 3) continue;
                var entityId = Convert.ToInt32(args[2]);

                foreach (var toBeDestroyed in GetEntities<EntityId>())
                    if (toBeDestroyed.Item1.entityId == entityId)
                        toBeDestroyed.Item1.gameObject.AddComponent<EntityDestroyed>();

                var message = $"Entity Id {entityId} has been destroyed.";
                EventUtility.CreateOnSendEvent(client.gameObject, message);
            }
        }
    }
}
