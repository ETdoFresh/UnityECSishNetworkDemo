using ECSish;

public class CreateClientOnSendEventOnInputFieldSubmitEvent : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var entity in GetEntities<InputFieldSubmitEvent>())
        {
            var message = entity.Item1.text;
            foreach (var client in GetEntities<Client>())
            {
                var sender = client.Item1.gameObject;
                ECSEvent.Create<OnSendEvent>(sender, message);
            }
        }
    }
}