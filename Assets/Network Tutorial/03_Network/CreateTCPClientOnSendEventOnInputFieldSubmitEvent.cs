using ECSish;

public class CreateTCPClientOnSendEventOnInputFieldSubmitEvent : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var entity in GetEntities<InputFieldSubmitEvent>())
        {
            var client = GetEntity<Client>();
            if (client != null)
            {
                var sender = client.Item1.gameObject;
                var message = entity.Item1.text;
                EventUtility.CreateOnSendEvent(sender.gameObject, message);
            }
        }
    }
}
