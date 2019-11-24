using ECSish;
using System;
using System.Linq;

public class ChatCommandSystem : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var entity in GetEntities<OnReceiveFromSessionEvent, TCPClientConnection>())
        {
            var args = entity.Item1.args;
            if (args.Length < 3) continue;

            var command = args[1].ToLower();
            if (command == "chat")
            {
                var chatMessage = entity.Item1.message;
                var sessionId = entity.Item1.sessionId;
                chatMessage = chatMessage.Substring(chatMessage.ToLower().IndexOf("chat") + 4).Trim();

                var session = GetEntities<Session>().Where(s => s.Item1.id == sessionId).Select(s => s.Item1).FirstOrDefault();
                if (session && !string.IsNullOrEmpty(session.nickname))
                    chatMessage = $"{session.nickname}: {chatMessage}";

                var client = entity.Item2;
                foreach (var otherSession in GetEntities<Session>())
                {
                    var otherClient = GetEntities<TCPClientConnection>().Where(c => c.Item1.connectionId == otherSession.Item1.connectionId).FirstOrDefault();
                    if (otherClient != null)
                        EventUtility.CreateOnSendEvent(otherClient.Item1.gameObject, $"{otherSession.Item1.id} {chatMessage}");
                }
            }
        }
    }
}
