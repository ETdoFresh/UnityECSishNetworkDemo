using ECSish;

public class OutputServerOnOpenEventToConsole : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var entity in GetEntities<ServerOnOpenEvent>())
        {
            var onOpenEvent = entity.Item1;
            var uiConsole = GetEntity<UIConsole>();
            if (uiConsole == null) continue;
            var textMesh = uiConsole.Item1.textMesh;
            var connection = onOpenEvent.connection;
            textMesh.text += $"{connection.host}:{connection.port} connected!\n";
        }
    }
}
