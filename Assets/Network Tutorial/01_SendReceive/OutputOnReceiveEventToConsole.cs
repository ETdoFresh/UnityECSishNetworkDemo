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

            if (message.Contains(" Input "))
            {
                foreach (var debug in GetEntities<DebugShowOnConsole>())
                    if (debug.Item1.showInput)
                    {
                        textMesh.text += $"{message}\n";
                        break;
                    }
            }
            else if (message.Contains("Update"))
            {
                foreach (var debug in GetEntities<DebugShowOnConsole>())
                    if (debug.Item1.showUpdate)
                    {
                        textMesh.text += $"{message}\n";
                        break;
                    }
            }
            else
            {
                textMesh.text += $"{message}\n";
            }
        }
    }
}
