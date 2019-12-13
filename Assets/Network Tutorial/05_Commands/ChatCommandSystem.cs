using ECSish;
using System.Linq;

public class ChatCommandSystem : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var entity in GetEntities<OnReceiveEvent, Session>())
        {
            var args = entity.Item1.Args;
            if (args.Length < 3) continue;

            var command = args[1].ToLower();
            var session = entity.Item2;
            if (command == "chat")
            {
                var chatMessage = entity.Item1.message;
                chatMessage = chatMessage.Substring(chatMessage.ToLower().IndexOf("chat") + 4).Trim();

                if (session && !string.IsNullOrEmpty(session.nickname))
                    chatMessage = $"{session.nickname}: {chatMessage}";

                var client = entity.Item2;
                foreach (var otherSession in GetEntities<Session>().Where(s => s.Item1 != session).Select(s => s.Item1))
                    ECSEvent.Create<OnSendEvent>(otherSession, chatMessage);
            }
        }
    }
}
