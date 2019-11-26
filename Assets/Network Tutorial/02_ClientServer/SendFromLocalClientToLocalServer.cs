using ECSish;

public class SendFromLocalClientToLocalServer : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var entity in GetEntities<OnSendEvent, Client>())
        {
            var message = entity.Item1.message;
            var client = entity.Item2;
            //var server = client.server;
            //EventSystem.Add(() =>
            //{
            //    var onReceiveEvent = server.gameObject.AddComponent<OnReceiveEvent>();
            //    onReceiveEvent.message = message;
            //    return onReceiveEvent;
            //});
        }
    }
}
