#if UNITY_EDITOR && UNITY_STANDALONE_LINUX

using System.IO;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

public class CopyFilesOnBuild : IPostprocessBuildWithReport
{
    public int callbackOrder { get { return 0; } }

    public void OnPostprocessBuild(BuildReport report)
    {
        var sourceDirectory = @".\BuildFiles\Linux\";
        var targetDirectory = @".\Build\Linux\";
        foreach (var file in Directory.GetFiles(sourceDirectory))
            File.Copy(file, Path.Combine(targetDirectory, Path.GetFileName(file)), true);
    }
}

#endif