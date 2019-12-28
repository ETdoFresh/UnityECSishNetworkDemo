using ECSish;
using UnityEngine;

public class ShowDebugOnConsoleSystem : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var uiConsole in GetEntities<UIConsole>())
        {
            foreach (var entity in GetEntities<OnSendEvent, DebugShowOnConsole>())
            {
                var debug = entity.Item2;
                var message = entity.Item1.message;
                var textMesh = uiConsole.Item1.textMesh;
                if (debug.showInput && message.Contains(" Input "))
                    textMesh.text += $"{message}\n";
                if (debug.showUpdate && message.Contains(" Update "))
                    textMesh.text += $"{message}\n";

                if (debug.showInputOnUnityConsole && message.Contains(" Input "))
                    Debug.Log(message);
                if (debug.showUpdateOnUnityConsole && message.Contains(" Update "))
                    Debug.Log(message);
            }

            foreach (var entity in GetEntities<OnReceiveEvent, DebugShowOnConsole>())
            {
                var debug = entity.Item2;
                var message = entity.Item1.message;
                var textMesh = uiConsole.Item1.textMesh;
                if (debug.showInput && message.Contains(" Input "))
                    textMesh.text += $"{message}\n";
                if (debug.showUpdate && message.Contains(" Update "))
                    textMesh.text += $"{message}\n";

                if (debug.showInputOnUnityConsole && message.Contains(" Input "))
                    Debug.Log(message);
                if (debug.showUpdateOnUnityConsole && message.Contains(" Update "))
                    Debug.Log(message);
            }
        }
    }
}
