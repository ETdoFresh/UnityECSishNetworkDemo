using ECSish;

public class CreateServerOnSendEventOnInputFieldSubmitEvent : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var entity in GetEntities<InputFieldSubmitEvent>())
        {
            var message = entity.Item1.text;
            foreach (var server in GetEntities<Server>())
            {
                var sender = server.Item1.gameObject;
                ECSEvent.Create<OnSendEvent>(sender, message);
            }
        }
    }
}
