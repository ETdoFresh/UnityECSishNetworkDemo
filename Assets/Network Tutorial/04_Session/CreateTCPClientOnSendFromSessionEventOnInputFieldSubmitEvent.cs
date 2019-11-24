using ECSish;
using System;

public class CreateTCPClientOnSendFromSessionEventOnInputFieldSubmitEvent : MonoBehaviourSystem
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

                var session = GetEntity<Session>();
                if (session != null
                    && !message.Contains("JoinLobby")
                    && !message.Contains("JoinRoom"))
                    message = $"{session.Item1.id} {message}";

                EventUtility.CreateOnSendEvent(sender.gameObject, message);
            }
        }
    }
}