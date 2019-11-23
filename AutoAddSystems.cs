using ECSish;
using System.Reflection;
using UnityEngine;

[ExecuteInEditMode]
public class AutoAddSystems : MonoBehaviour
{
    public Coroutine coroutine;

#if UNITY_EDITOR
    // TODO: Find a better way to AutoAddSystems instead of each Update
    void Update()
    {
        if (Application.isPlaying)
            return;

        //Debug.Log("Running AutoAddSystems...");
        foreach (var type in Assembly.GetAssembly(GetType()).GetTypes())
        {

            if (type.IsSubclassOf(typeof(MonoBehaviourSystem)))
            {
                var component = GetComponentInChildren(type);
                if (!component)
                {
                    Debug.Log($"AutoAddSystems: Adding type {type.FullName}");
                    gameObject.AddComponent(type);
                }
            }
        }
    }
#endif
}
