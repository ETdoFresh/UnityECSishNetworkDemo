using ECSish;

public class SendFromLocalClientToLocalServer : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var entity in GetEntities<OnSendEvent, LocalClient>())
        {
            var message = entity.Item1.message;
            var client = entity.Item2;
            var server = client.server;
            ECSEvent.Create<OnReceiveEvent>(server, message);
        }
    }
}
