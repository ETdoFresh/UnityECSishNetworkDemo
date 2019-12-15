using ECSish;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MonoBehaviourComponentData))]
public class MonoBehavoiourComponentDataEditor : Editor
{
    private void Awake()
    {
        MethodInfo SetIconForObject = typeof(EditorGUIUtility).GetMethod("SetIconForObject", BindingFlags.Static | BindingFlags.NonPublic);
        MethodInfo CopyMonoScriptIconToImporters = typeof(MonoImporter).GetMethod("CopyMonoScriptIconToImporters", BindingFlags.Static | BindingFlags.NonPublic);

        //Monoscript script = whatever;
        //Texture2D icon = whatever;

        //SetIconForObject.Invoke(null, new[] { script, icon });
        //CopyMonoScriptToImporters.invoke(null, script);
    }
}
