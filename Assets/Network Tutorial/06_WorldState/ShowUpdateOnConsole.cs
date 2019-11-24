using ECSish;

public class ShowUpdateOnConsole : MonoBehaviourSystem
{
    private void Update()
    {
        foreach(var entity in GetEntities<OnReceiveEvent, Client>())
        {
            var message = entity.Item1.message;
            if (message.Contains(" Update"))
            {
                var textMesh = GetEntity<UIConsole>().Item1.textMesh;
                textMesh.text += $"{message}\n";
            }
        }

        foreach (var entity in GetEntities<OnSendEvent, TCPClientConnection>())
        {
            var message = entity.Item1.message;
            if (message.Contains(" Update"))
            {
                var textMesh = GetEntity<UIConsole>().Item1.textMesh;
                textMesh.text += $"<#808080>{message}</color>\n";
            }
        }
    }
}
