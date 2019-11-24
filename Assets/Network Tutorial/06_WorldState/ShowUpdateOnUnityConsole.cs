using ECSish;
using UnityEngine;

public class ShowUpdateOnUnityConsole : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var entity in GetEntities<OnReceiveEvent, Client>())
        {
            var message = entity.Item1.message;
            if (message.Contains(" Update"))
                Debug.Log(message);
        }

        foreach (var entity in GetEntities<OnSendEvent, TCPClientConnection>())
        {
            var message = entity.Item1.message;
            if (message.Contains(" Update"))
                Debug.Log(message);
        }
    }
}