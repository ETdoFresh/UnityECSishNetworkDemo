using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Object script;
    public int MyValue { get; set; }
}

[CustomEditor(typeof(Test))]
public class TestEditor : Editor
{
    private void Awake()
    {
        MethodInfo SetIconForObject = typeof(EditorGUIUtility).GetMethod("SetIconForObject", BindingFlags.Static | BindingFlags.NonPublic);
        MethodInfo CopyMonoScriptIconToImporters = typeof(MonoImporter).GetMethod("CopyMonoScriptIconToImporters", BindingFlags.Static | BindingFlags.NonPublic);
        var myTarget = (Test)target;
        var script = MonoScript.FromMonoBehaviour(myTarget);
        var icon = Resources.Load<Texture2D>("ECSishComponentData");

        var ty = typeof(EditorGUIUtility);
        var mi = ty.GetMethod("SetIconForObject", BindingFlags.NonPublic | BindingFlags.Static);
        mi.Invoke(null, new object[] { script, icon });
        mi.Invoke(null, new object[] { myTarget.gameObject, null });

        //Monoscript script = scr;
        //Texture2D icon = whatever;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Test myTarget = (Test)target;

        myTarget.MyValue = EditorGUILayout.IntSlider("Val-you", myTarget.MyValue, 1, 10);
    }
}