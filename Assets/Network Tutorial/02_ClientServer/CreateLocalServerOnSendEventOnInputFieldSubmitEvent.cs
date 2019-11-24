using ECSish;

public class CreateLocalServerOnSendEventOnInputFieldSubmitEvent : MonoBehaviourSystem
{
    private void Update()
    {
        var entities = GetEntities<InputFieldSubmitEvent>();
        foreach (var entity in entities)
        {
            var server = GetEntity<LocalServer>();
            if (server != null)
            {
                var sender = server.Item1.gameObject;
                var message = entity.Item1.text;
                EventUtility.CreateOnSendEvent(sender.gameObject, message);
            }
        }
    }
}
