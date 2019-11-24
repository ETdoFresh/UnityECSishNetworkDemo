using ECSish;
using System;
using System.Linq;

public class WhisperCommandSystem : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var entity in GetEntities<OnReceiveFromSessionEvent, TCPClientConnection>())
        {
            var args = entity.Item1.args;
            if (args.Length < 3) continue;

            var sessionId = Convert.ToInt32(args[0]);
            var command = args[1].ToLower();

            if (command == "whisper")
            {
                var reciepient = args[2];

                var chatMessage = entity.Item1.message;
                chatMessage = chatMessage.Substring(chatMessage.ToLower().IndexOf("whisper") + 7);
                chatMessage = chatMessage.Substring(chatMessage.IndexOf(reciepient) + reciepient.Length);
                chatMessage = chatMessage.Trim();

                var session = GetEntities<Session>().Where(s => s.Item1.id == sessionId).Select(s => s.Item1).FirstOrDefault();
                if (session && !string.IsNullOrEmpty(session.nickname))
                    chatMessage = $"{session.nickname} (whisper): {chatMessage}";

                var reciepientSession = GetEntities<Session>()
                    .Where(s => s.Item1.id.ToString() == reciepient || s.Item1.nickname == reciepient)
                    .Select(s => s.Item1).FirstOrDefault();

                var otherClient = GetEntities<TCPClientConnection>().Where(c => c.Item1.connectionId == reciepientSession.connectionId).FirstOrDefault();
                if (otherClient != null)
                    EventUtility.CreateOnSendEvent(otherClient.Item1.gameObject, $"{reciepientSession.id} {chatMessage}");
            }
        }
    }
}
