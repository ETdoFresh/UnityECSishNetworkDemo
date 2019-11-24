using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public float fadeTime = 0.5f;
    public Texture2D texture2D;
    public float alpha;

    public void LoadScene(string sceneName) =>
        SceneManager.LoadScene(sceneName);

    public void LoadScene(Object scene) =>
        SceneManager.LoadScene(scene.name);

    public void LoadScene(int sceneBuildIndex) =>
        SceneManager.LoadScene(sceneBuildIndex);

    public void CrossFadeScene(string sceneName)
    {
        StartCoroutine(CrossFadeSceneCoroutine(sceneName));
    }

    private IEnumerator CrossFadeSceneCoroutine(string sceneName)
    {
        if (!texture2D)
            texture2D = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        yield return new WaitForEndOfFrame();
        texture2D.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, false);
        texture2D.Apply();
        LoadScene(sceneName);

        var rate = 1.0f / fadeTime;
        for (alpha = 1.0f; alpha > 0.0; alpha -= Time.deltaTime * rate)
            yield return null;

        Destroy(texture2D);
    }

    private void OnGUI()
    {
        if (!texture2D)
            return;

        GUI.depth = -9999999;
        var color = GUI.color;
        color.a = alpha;
        GUI.color = color;
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), texture2D);
    }
}