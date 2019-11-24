using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneExtension
{
    public static T FindObjectOfType<T>(this Scene scene, bool includeInactive = false)
    {
        foreach (var obj in FindObjectsOfType<T>(scene, includeInactive))
            return obj;
        return default;
    }

    public static IEnumerable<T> FindObjectsOfType<T>(this Scene scene, bool includeInactive = false)
    {
        GameObject[] gameObjects;
        try { gameObjects = scene.GetRootGameObjects(); }
        catch { yield break; }

        if (scene.IsValid())
            foreach (var gameObject in gameObjects)
                foreach (var component in gameObject.GetComponentsInChildren<T>(includeInactive))
                    yield return component;
    }

    public static float GetAxisRaw(this Scene scene, string axisName)
    {
        var input = Input.GetAxisRaw(axisName);
        if (input == 0)
            return input;

        var focusScene = Object.FindObjectOfType<SplitScreenPanel>();
        if (focusScene == null)
            return input;
        if (focusScene.gameObject.scene == scene)
            return input;
        else
            return 0;
    }

    public static bool GetButtonDown(this Scene scene, string buttonName)
    {
        var input = Input.GetButtonDown(buttonName);
        if (input == false)
            return input;

        var focusScene = Object.FindObjectOfType<SplitScreenPanel>();
        if (focusScene == null)
            return input;
        if (focusScene.gameObject.scene == scene)
            return input;
        else
            return false;
    }

    // Because moving objects cause GameObject to disable and re-enable
    public static GameObject Instantiate(this Scene scene, GameObject prefab)
    {
        var localPrefab = Object.Instantiate(prefab);
        localPrefab.SetActive(false);
        var instance = Object.Instantiate(localPrefab);
        Object.DestroyImmediate(localPrefab);
        SceneManager.MoveGameObjectToScene(instance, scene);
        instance.SetActive(true);
        return instance;
    }

    public static GameObject NewGameObject(this Scene scene)
    {
        var instance = new GameObject();
        SceneManager.MoveGameObjectToScene(instance, scene);
        return instance;
    }
}