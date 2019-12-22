using ECSish;

public class OutputServerOnServerOpenEventToConsole : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var entity in GetEntities<ServerOnServerOpenEvent>())
        {
            var server = entity.Item1;
            var uiConsole = GetEntity<UIConsole>();
            if (uiConsole == null) continue;
            var textMesh = uiConsole.Item1.textMesh;
            textMesh.text += $"{server.name} is listening...\n";
        }
    }
}
