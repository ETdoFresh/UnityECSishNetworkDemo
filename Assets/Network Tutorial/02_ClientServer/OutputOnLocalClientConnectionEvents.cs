using ECSish;

public class OutputOnLocalClientConnectionEvents : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var entity in GetEntities<OnLocalClientAccepted>())
        {
            var uiConsole = GetEntity<UIConsole>();
            if (uiConsole == null) continue;

            var message = "Client Connected!";
            var textMesh = uiConsole.Item1.textMesh;
            textMesh.text += $"{message}\n";
        }

        foreach (var entity in GetEntities<OnLocalClientDisconnected>())
        {
            var uiConsole = GetEntity<UIConsole>();
            if (uiConsole == null) continue;

            var message = "Client Disconnected!";
            var textMesh = uiConsole.Item1.textMesh;
            textMesh.text += $"{message}\n";
        }
    }
}