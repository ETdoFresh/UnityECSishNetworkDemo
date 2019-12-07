using ECSish;

public class OutputOnAcceptedEventToConsole : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var entity in GetEntities<OnAcceptedEvent>())
        {
            var onAcceptedEvent = entity.Item1;
            var uiConsole = GetEntity<UIConsole>();
            if (uiConsole == null) continue;
            var textMesh = uiConsole.Item1.textMesh;
            var connection = onAcceptedEvent.connection;
            textMesh.text += $"{connection.host}:{connection.port} connected!\n";
        }
    }
}
