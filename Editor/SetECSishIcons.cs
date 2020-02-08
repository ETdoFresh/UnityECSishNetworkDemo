using ECSish;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class SetECSishIcons : AssetPostprocessor
{
    private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        MethodInfo GetIconForObject = typeof(EditorGUIUtility).GetMethod("GetIconForObject", BindingFlags.Static | BindingFlags.NonPublic);
        MethodInfo SetIconForObject = typeof(EditorGUIUtility).GetMethod("SetIconForObject", BindingFlags.Static | BindingFlags.NonPublic);
        MethodInfo CopyMonoScriptIconToImporters = typeof(MonoImporter).GetMethod("CopyMonoScriptIconToImporters", BindingFlags.Static | BindingFlags.NonPublic);
        var importedScripts = importedAssets.Select(assetPath => AssetDatabase.LoadAssetAtPath<MonoScript>(assetPath)).Where(asset => asset);

        var componentDataIcon = Resources.Load<Texture2D>("ECSishComponentData");
        foreach (var script in importedScripts.Where(s => s.GetClass() != null && s.GetClass().IsSubclassOf(typeof(MonoBehaviourComponentData))))
        {
            var icon = GetIconForObject.Invoke(null, new[] { script });
            if (icon == null || !icon.Equals(componentDataIcon))
            {
                SetIconForObject.Invoke(null, new object[] { script, componentDataIcon });
                CopyMonoScriptIconToImporters.Invoke(null, new object[] { script });
            }
        }

        var systemIcon = Resources.Load<Texture2D>("ECSishSystem");
        foreach (var script in importedScripts.Where(s => s && s.GetClass() != null && s.GetClass().IsSubclassOf(typeof(MonoBehaviourSystem))))
        {
            var icon = GetIconForObject.Invoke(null, new[] { script });
            if (icon == null || !icon.Equals(systemIcon))
            {
                SetIconForObject.Invoke(null, new object[] { script, systemIcon });
                CopyMonoScriptIconToImporters.Invoke(null, new object[] { script });
            }
        }

        var entityIcon = Resources.Load<Texture2D>("ECSishEntity");
    }
}