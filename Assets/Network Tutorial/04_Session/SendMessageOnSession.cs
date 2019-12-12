using ECSish;

public class SendMessageOnSession : MonoBehaviourSystem
{
    private void Update()
    {
        foreach(var entity in GetEntities<OnSendEvent, Session>())
        {
            var sendEvent = entity.Item1;
            var session = entity.Item2;
            var message = $"{session.id} {sendEvent.message}";

            if (session.connection != null)
                ECSEvent.Create<OnSendEvent>(session.connection.gameObject, message);
            else
            {
                var client = GetEntity<Client>();
                if (client != null)
                    ECSEvent.Create<OnSendEvent>(client.Item1.gameObject, message);
            }
        }
    }
}