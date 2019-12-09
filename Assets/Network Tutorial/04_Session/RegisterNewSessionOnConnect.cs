using ECSish;

public class RegisterNewSessionOnConnect : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var entity in GetEntities<OnConnectedEvent, Client>())
        {
            var client = entity.Item2;
            if (!client) continue;

            var message = "RegisterNewSession";
#if UNITY_EDITOR
            message += " Editor";
#elif UNITY_WEBGL
            message += " WebGL";
#elif UNITY_STANDALONE_WIN
            message += " Windows";
#elif UNITY_STANDALONE_OSX
            message += " OSX";
#elif UNITY_STANDALONE_LINUX
            message += " Linux";
#endif
            EventUtility.CreateOnSendEvent(client.gameObject, message);
        }
    }
}
