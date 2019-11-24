using ECSish;

public class OutputOnSendEventToConsole : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var entity in GetEntities<OnSendEvent>())
        {
            var message = entity.Item1.message;

            var uiConsole = GetEntity<UIConsole>();
            if (uiConsole == null) continue;
            var textMesh = uiConsole.Item1.textMesh;

            // TODO: Remove this with a much better system
            if (message.Contains(" Input ")) continue;
            if (message.Contains(" Update")) continue;

            textMesh.text += $"<#808080>{message}</color>\n";
        }
    }
}
