using ECSish;

public class CreateMessageOnClientSessionOnInputFieldSubmit : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var inputEntity in GetEntities<InputFieldSubmitEvent>())
        {
            var message = inputEntity.Item1.text;
            foreach (var sessionEntity in GetEntities<Session>())
            {
                var session = sessionEntity.Item1;
                EventUtility.CreateOnSendEvent(session.gameObject, message);
            }
        }
    }
}
