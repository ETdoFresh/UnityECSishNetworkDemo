using ECSish;

public class OutputClientConnectionEventsToConsole : MonoBehaviourSystem
{
    private void Update()
    {
        var textMesh = GetEntity<UIConsole>().Item1.textMesh;
        if (!textMesh) return;

        foreach (var entity in GetEntities<ClientOnOpenEvent>())
        {
            var client = entity.Item1;
            textMesh.text += $"{client.name} connected!\n";
        }

        foreach (var entity in GetEntities<ClientOnCloseEvent>())
        {
            var client = entity.Item1;
            textMesh.text += $"{client.name} disconnected!\n";
        }
    }
}
