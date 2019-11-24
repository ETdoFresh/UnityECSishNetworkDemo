using ECSish;

public class CreateLocalClientOnSendEventOnInputFieldSubmitEvent : MonoBehaviourSystem
{
    private void Update()
    {
        var entities = GetEntities<InputFieldSubmitEvent>();
        foreach (var entity in entities)
        {
            var client = GetEntity<LocalClient>();
            if (client != null)
            {
                var sender = client.Item1.gameObject;
                var message = entity.Item1.text;
                EventUtility.CreateOnSendEvent(sender.gameObject, message);
            }
        }
    }
}