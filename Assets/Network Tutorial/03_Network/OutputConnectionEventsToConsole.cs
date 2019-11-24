using ECSish;

public class OutputConnectionEventsToConsole : MonoBehaviourSystem
{
    private void Update()
    {
        var textMesh = GetEntity<UIConsole>().Item1.textMesh;
        if (!textMesh) return;

        foreach (var entity in GetEntities<OnConnectingEvent>())
        {
            var client = entity.Item1;
            textMesh.text += $"{client.name} is connecting...\n";
        }

        foreach (var entity in GetEntities<OnConnectedEvent>())
        {
            var client = entity.Item1;
            textMesh.text += $"{client.name} connected!\n";
        }

        foreach (var entity in GetEntities<OnCouldNotConnectEvent>())
        {
            var client = entity.Item1;
            textMesh.text += $"{client.name} could not connect!\n";
        }

        foreach (var entity in GetEntities<OnDisconnectedEvent>())
        {
            var client = entity.Item1;
            textMesh.text += $"{client.name} disconnected!\n";
        }
    }
}
