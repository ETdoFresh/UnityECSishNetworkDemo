using ECSish;
using System;

public class ExitCommandSystem : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var entity in GetEntities<OnSendEvent>())
        {
            var args = entity.Item1.message.Split(new[] { " " }, StringSplitOptions.None);
            if (args.Length == 0 || args.Length > 2) continue;
            var command = args.Length == 1 ? args[0].ToLower() : args[1].ToLower();
            if (command == "exit" || command == "quit")
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBGL
                UnityEngine.Application.OpenURL("https://www.etdofresh.com");
#else
                UnityEngine.Application.Quit();
#endif
            }
        }
    }
}