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
                ECSEvent.Add(() =>
                {
                    var onReceiveEvent = client.gameObject.AddComponent<OnReceiveEvent>();
                    onReceiveEvent.message = message;
                    return onReceiveEvent;
                });
        }
    }
}
