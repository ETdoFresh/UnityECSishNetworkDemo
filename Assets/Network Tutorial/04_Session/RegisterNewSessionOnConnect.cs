using ECSish;
using System;
using System.Net.Sockets;
using System.Text;

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
            var bytes = Encoding.UTF8.GetBytes(message);
            EventUtility.CreateOnSendEvent(client.gameObject, message);
        }
    }
}
