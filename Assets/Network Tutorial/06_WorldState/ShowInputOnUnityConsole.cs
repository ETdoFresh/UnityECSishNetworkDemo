using ECSish;
using UnityEngine;

public class ShowInputOnUnityConsole : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var entity in GetEntities<OnSendEvent, Client>())
        {
            var message = entity.Item1.message;
            if (message.Contains(" Input "))
                Debug.Log(message);
        }

        foreach (var entity in GetEntities<OnReceiveEvent, TCPClientConnection>())
        {
            var message = entity.Item1.message;
            if (message.Contains(" Input "))
                Debug.Log(message);
        }
    }
}