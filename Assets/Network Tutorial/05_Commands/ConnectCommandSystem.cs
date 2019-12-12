using ECSish;

public class ConnectCommandSystem : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var entity in GetEntities<OnSendEvent, Client>())
        {
            var args = entity.Item1.message.Split(' ');
            if (args.Length < 1) continue;

            var command = args[0].ToLower();
            if (command == "connect")
                ECSEvent.Create<OnConnectingEvent>(entity.Item2);
        }
    }
}
