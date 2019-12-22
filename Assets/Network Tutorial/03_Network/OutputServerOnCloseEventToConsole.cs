using ECSish;

public class OutputServerOnCloseEventToConsole : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var entity in GetEntities<ServerOnCloseEvent>())
        {
            var onDisconnectedEvent = entity.Item1;

            var uiConsole = GetEntity<UIConsole>();
            if (uiConsole == null) continue;
            var textMesh = uiConsole.Item1.textMesh;

            textMesh.text += $"{onDisconnectedEvent.connection.host}:{onDisconnectedEvent.connection.port} disconnected!\n";
        }
    }
}
