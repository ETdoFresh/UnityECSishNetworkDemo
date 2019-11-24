using ECSish;
using UnityEngine.SceneManagement;

public class LoadSceneSystemOnEnable : MonoBehaviourSystem
{
    private void Update()
    {
        foreach(var entity in GetEntities<LoadScene, OnEnableEvent>())
        {
            var scenes = entity.Item1.scenes;
            foreach (var scene in scenes)
                SceneManager.LoadScene(scene, LoadSceneMode.Additive);
        }
    }
}
