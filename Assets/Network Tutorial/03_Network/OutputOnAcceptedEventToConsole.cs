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
            textMesh.text += $"{onAcceptedEvent.connection.host}:{onAcceptedEvent.connection.port} connected!\n";
        }
    }
}
