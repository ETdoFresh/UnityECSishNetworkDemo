using ECSish;

public class OutputOnReceiveEventToConsole : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var entity in GetEntities<OnReceiveEvent>())
        {
            var message = entity.Item1.message;
            var uiConsole = GetEntity<UIConsole>();
            if (uiConsole == null) continue;

            var textMesh = uiConsole.Item1.textMesh;

            // TODO: Remove this with a much better system
            if (message.Contains(" Input ")) continue; 
            if (message.Contains(" Update")) continue;

            textMesh.text += $"{message}\n";
        }
    }
}
