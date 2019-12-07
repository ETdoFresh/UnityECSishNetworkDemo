using ECSish;

public class OutputOnDisconnectedFromTCPServerEventToConsole : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var entity in GetEntities<OnDisconnectedFromServerEvent>())
        {
            var onDisconnectedEvent = entity.Item1;

            var uiConsole = GetEntity<UIConsole>();
            if (uiConsole == null) continue;
            var textMesh = uiConsole.Item1.textMesh;

            textMesh.text += $"{onDisconnectedEvent.connection.host}:{onDisconnectedEvent.connection.port} disconnected!\n";
        }
    }
}
