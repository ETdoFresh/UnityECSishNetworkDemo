using ECSish;

public class CreateLocalPeerOnSendEventOnInputFieldSubmitEvent : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var entity in GetEntities<InputFieldSubmitEvent>())
        {
            var peerToPeerClient = GetEntity<LocalPeerClient>();
            if (peerToPeerClient != null)
            {
                var sender = peerToPeerClient.Item1.gameObject;
                var message = entity.Item1.text;
                EventUtility.CreateOnSendEvent(sender.gameObject, message);
            }
        }
    }
}
