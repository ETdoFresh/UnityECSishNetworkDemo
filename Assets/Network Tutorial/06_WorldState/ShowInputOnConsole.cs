using ECSish;

public class ShowInputOnConsole : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var entity in GetEntities<OnSendEvent, Client>())
        {
            var message = entity.Item1.message;
            if (message.Contains(" Input "))
            {
                var textMesh = GetEntity<UIConsole>().Item1.textMesh;
                textMesh.text += $"{message}\n";
            }
        }

        foreach (var entity in GetEntities<OnReceiveEvent, TCPClientConnection>())
        {
            var message = entity.Item1.message;
            if (message.Contains(" Input "))
            {
                var textMesh = GetEntity<UIConsole>().Item1.textMesh;
                textMesh.text += $"{message}\n";
            }
        }
    }
}
