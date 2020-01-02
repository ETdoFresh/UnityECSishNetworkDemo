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

            if (message.Contains(" Input "))
            {
                foreach (var debug in GetEntities<DebugShowOnConsole>())
                    if (debug.Item1.showInput)
                    {
                        textMesh.text += $"<#808080>{message}</color>\n";
                        break;
                    }
            }
            else if (message.Contains("Update"))
            {
                foreach (var debug in GetEntities<DebugShowOnConsole>())
                    if (debug.Item1.showUpdate)
                    {
                        textMesh.text += $"<#808080>{message}</color>\n";
                        break;
                    }
            }
            else
            {
                textMesh.text += $"<#808080>{message}</color>\n";
            }
        }
    }
}
