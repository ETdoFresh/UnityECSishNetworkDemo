using ECSish;
using System;
using UnityEngine.SceneManagement;

public class RestartCommandSystem : MonoBehaviourSystem 
{
    private void Update()
    {
        foreach(var entity in GetEntities<OnSendEvent>())
        {
            var args = entity.Item1.message.Split(new[] { " " }, StringSplitOptions.None);
            if (args.Length == 0 || args.Length > 2) continue;
            var command = args.Length == 1 ? args[0].ToLower() : args[1].ToLower();
            if (command == "restart")
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
