using ECSish;
using System;

public class DisconnectCommandSystem : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var entity in GetEntities<OnSendEvent>())
        {
            var args = entity.Item1.message.Split(new[] { " " }, StringSplitOptions.None);
            if (args.Length == 0 || args.Length > 2) continue;
            var command = args.Length == 1 ? args[0].ToLower() : args[1].ToLower();
            if (command == "disconnect")
            {
                var clientEntity = entity.Item1.gameObject;
                var client = clientEntity.GetComponent<Client>();
                if (client)
                    ECSEvent.Create<OnDisconnectedEvent>(client);
            }
        }
    }
}
