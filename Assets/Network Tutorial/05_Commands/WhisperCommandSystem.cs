using ECSish;
using System;
using System.Linq;

public class WhisperCommandSystem : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var entity in GetEntities<OnReceiveEvent, Session>())
        {
            var args = entity.Item1.Args;
            if (args.Length < 3) continue;

            var session = entity.Item2;
            var sessionId = session.id;
            var command = args[1].ToLower();

            if (command == "whisper")
            {
                var reciepient = args[2];

                var chatMessage = entity.Item1.message;
                chatMessage = chatMessage.Substring(chatMessage.ToLower().IndexOf("whisper") + 7);
                chatMessage = chatMessage.Substring(chatMessage.IndexOf(reciepient) + reciepient.Length);
                chatMessage = chatMessage.Trim();

                if (session && !string.IsNullOrEmpty(session.nickname))
                    chatMessage = $"{session.nickname} (whisper): {chatMessage}";

                var reciepientSession = GetEntities<Session>()
                    .Where(s => s.Item1.id.ToString() == reciepient || s.Item1.nickname == reciepient)
                    .Select(s => s.Item1).FirstOrDefault();

                if (reciepientSession != null)
                    ECSEvent.Create<OnSendEvent>(reciepientSession, chatMessage);
            }
        }
    }
}
