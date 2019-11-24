using ECSish;

public class OutputOnListeningEventToConsole : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var entity in GetEntities<OnListeningEvent>())
        {
            var server = entity.Item1;
            var uiConsole = GetEntity<UIConsole>();
            if (uiConsole == null) continue;
            var textMesh = uiConsole.Item1.textMesh;
            textMesh.text += $"{server.name} is listening...\n";
        }
    }
}
