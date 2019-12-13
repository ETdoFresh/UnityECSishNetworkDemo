using ECSish;

public class SendFromLocalServerToLocalClients : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var entity in GetEntities<OnSendEvent, LocalServer>())
        {
            var message = entity.Item1.message;
            var server = entity.Item2;
            foreach (var client in server.clients)
                ECSEvent.Create<OnReceiveEvent>(client, message);
        }
    }
}
