using ECSish;

public class OutputOnErrorEventToConsole : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var entity in GetEntities<OnErrorEvent>())
        {
            var message = entity.Item1.Message;

            var uiConsole = GetEntity<UIConsole>();
            if (uiConsole == null) continue;
            var textMesh = uiConsole.Item1.textMesh;

            textMesh.text += $"<#800000>{message}</color>\n";
        }
    }
}