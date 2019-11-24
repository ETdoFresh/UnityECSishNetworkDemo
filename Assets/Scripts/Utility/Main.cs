using UnityEngine;

public class Main : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void LoadMain()
    {
        var prefab = Resources.Load("Main") as GameObject;
        GameObject main = Instantiate(prefab);
        main.name = prefab.name;
        DontDestroyOnLoad(main);
    }
}