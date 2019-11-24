using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHistory : MonoBehaviour
{
    public List<string> ignoreList = new List<string>();
    public List<string> previousScenes = new List<string>();
    public SceneLoader sceneLoader;
    public GameObject backButton;
    public bool goingBack = false;

    private void GetVariables()
    {
        if (!sceneLoader) sceneLoader = GetComponent<SceneLoader>();
    }

    private void OnValidate() => GetVariables();
    private void Awake() => GetVariables();

    private void OnEnable()
    {
        backButton.SetActive(previousScenes.Count > 0);
        SceneManager.sceneUnloaded += AddNewPreviousScene;
    }

    private void OnDisable()
    {
        backButton.SetActive(false);
        SceneManager.sceneUnloaded -= AddNewPreviousScene;
    }

    public void LoadPreviousScene()
    {
        var previousScene = previousScenes[previousScenes.Count - 1];
        previousScenes.RemoveAt(previousScenes.Count - 1);
        backButton.SetActive(previousScenes.Count > 0);
        goingBack = true;
        sceneLoader.CrossFadeScene(previousScene);
    }

    private void AddNewPreviousScene(Scene scene)
    {
        if (!goingBack)
        {
            if (SceneManager.GetActiveScene() != scene)
                return;

            if (ignoreList.Contains(scene.name))
                return;

            previousScenes.Add(scene.name);
            backButton.SetActive(true);
        }
        else
            goingBack = false;
    }
}