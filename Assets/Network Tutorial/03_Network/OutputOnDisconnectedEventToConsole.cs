using ECSish;

public class OutputOnDisconnectedEventToConsole : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var entity in GetEntities<OnDisconnectedEvent>())
        {
            var client = entity.Item1;
            var textMesh = GetEntity<UIConsole>().Item1.textMesh;
            textMesh.text += $"{client.name} disconnected!\n";
        }
    }
}