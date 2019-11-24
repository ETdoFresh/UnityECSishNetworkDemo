#if UNITY_EDITOR

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

public class PlatformMenu
{
    [MenuItem("Platform/Build and Upload Open Scenes [Linux]")]
    static void BuildAndUploadLinuxServer()
    {
        var previousTarget = EditorUserBuildSettings.activeBuildTarget;
        var previousTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
        var buildPlayerOptions = new BuildPlayerOptions
        {
            scenes = OpenScenes().Select(s => s.path).ToArray(),
            targetGroup = BuildTargetGroup.Standalone,
            target = BuildTarget.StandaloneLinux64,
            locationPathName = @"Build\Linux\NetworkStringTester.x86_64",
            options = BuildOptions.EnableHeadlessMode | BuildOptions.ShowBuiltPlayer | BuildOptions.Development
        };
        BuildPipeline.BuildPlayer(buildPlayerOptions);
        Process.Start(@"Build\Linux\UploadToETdoFresh.cmd");
        EditorUserBuildSettings.SwitchActiveBuildTarget(previousTargetGroup, previousTarget);
    }

    [MenuItem("Platform/Build and Run Open Scenes [Windows Headless]")]
    static void BuildAndRunOpenScenesWindowsHeadless()
    {
        var previousTarget = EditorUserBuildSettings.activeBuildTarget;
        var previousTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
        var buildPlayerOptions = new BuildPlayerOptions
        {
            scenes = OpenScenes().Select(s => s.path).ToArray(),
            targetGroup = BuildTargetGroup.Standalone,
            target = BuildTarget.StandaloneWindows64,
            locationPathName = @"Build\WindowsServer\Server.exe",
            options = BuildOptions.AutoRunPlayer | BuildOptions.Development | BuildOptions.EnableHeadlessMode
        };
        BuildPipeline.BuildPlayer(buildPlayerOptions);
        EditorUserBuildSettings.SwitchActiveBuildTarget(previousTargetGroup, previousTarget);
    }

    [MenuItem("Platform/Build and Run Open Scenes [Windows]")]
    static void BuildAndRunOpenScenesWindows()
    {
        var previousTarget = EditorUserBuildSettings.activeBuildTarget;
        var previousTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
        var buildPlayerOptions = new BuildPlayerOptions
        {
            scenes = OpenScenes().Select(s => s.path).ToArray(),
            targetGroup = BuildTargetGroup.Standalone,
            target = BuildTarget.StandaloneWindows64,
            locationPathName = @"Build\WindowsServer\Server.exe",
            options = BuildOptions.AutoRunPlayer | BuildOptions.Development
        };
        BuildPipeline.BuildPlayer(buildPlayerOptions);
        EditorUserBuildSettings.SwitchActiveBuildTarget(previousTargetGroup, previousTarget);
    }

    [MenuItem("Platform/Build and Run Open Scenes [WebGL]")]
    static void BuildAndRunOpenScenesWebGL()
    {
        var previousTarget = EditorUserBuildSettings.activeBuildTarget;
        var previousTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
        var buildPlayerOptions = new BuildPlayerOptions
        {
            scenes = OpenScenes().Select(s => s.path).ToArray(),
            targetGroup = BuildTargetGroup.WebGL,
            target = BuildTarget.WebGL,
            locationPathName = @"Build\WebGL",
            options = BuildOptions.AutoRunPlayer | BuildOptions.Development
        };
        BuildPipeline.BuildPlayer(buildPlayerOptions);
        EditorUserBuildSettings.SwitchActiveBuildTarget(previousTargetGroup, previousTarget);
    }

    [MenuItem("Platform/Switch Platform to Windows Standalone")]
    static void SwitchPlatformToWindows()
    {
        SwitchTo(BuildTarget.StandaloneWindows64);
    }

    [MenuItem("Platform/Switch Platform to Windows Standalone", true)]
    static bool SwitchPlatformToWindowsValidate()
    {
        return EditorUserBuildSettings.activeBuildTarget != BuildTarget.StandaloneWindows64;
    }

    [MenuItem("Platform/Switch Platform to WebGL")]
    static void SwitchPlatformToWebGL()
    {
        SwitchTo(BuildTarget.WebGL);
    }

    [MenuItem("Platform/Switch Platform to WebGL", true)]
    static bool SwitchPlatformToWebGLValidate()
    {
        return EditorUserBuildSettings.activeBuildTarget != BuildTarget.WebGL;
    }

    public static void SwitchTo(BuildTarget targetPlatform)
    {
        var currentPlatform = EditorUserBuildSettings.activeBuildTarget;
        if (currentPlatform == targetPlatform)
            return;

        // Don't switch when compiling
        if (EditorApplication.isCompiling)
        {
            Debug.LogWarning("Could not switch platform because unity is compiling");
            return;
        }

        // Don't switch while playing
        if (EditorApplication.isPlayingOrWillChangePlaymode)
        {
            Debug.LogWarning("Could not switch platform because unity is in playMode");
            return;
        }

        //Debug.Log("Switching platform from " + currentPlatform + " to " + targetPlatform);

        //string libDir = "Library";
        //string libDirCurrent = libDir + "_" + currentPlatform;
        //string libDirTarget = libDir + "_" + targetPlatform;

        //// If target dir doesn't exist yet, make a copy of the current one
        //if (!Directory.Exists(libDirTarget))
        //{
        //    Debug.Log("Making a copy of " + libDir + " because " + libDirTarget + " doesn't exist yet");
        //    CopyFilesRecursively(new DirectoryInfo(libDir), new DirectoryInfo(libDirTarget));
        //}

        //// Safety check, libDirCurrent shouldn't exist (current data is stored in libDir)
        //if (Directory.Exists(libDirCurrent))
        //    Directory.Delete(libDirCurrent, true);

        //// Rename dirs
        //Directory.Move(libDir, libDirCurrent);
        //Directory.Move(libDirTarget, libDir);

        var targetBuildGroup = targetPlatform == BuildTarget.WebGL ? BuildTargetGroup.WebGL
            : BuildTargetGroup.Standalone;

        if (targetPlatform == BuildTarget.StandaloneLinux64)
            EditorUserBuildSettings.enableHeadlessMode = true;
        else
            EditorUserBuildSettings.enableHeadlessMode = false;

        //// Disable/Enable scenes until the server
        //EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
        //if (targetPlatform == BuildTarget.StandaloneLinux64)
        //    foreach (var scene in scenes)
        //        if (scene.path.Contains("10d_Server") && scene.enabled)
        //            break;
        //        else
        //            scene.enabled = false;
        //else
        //    foreach (var scene in scenes)
        //        if (scene.path.Contains("10d_Server") && scene.enabled)
        //            break;
        //        else
        //            scene.enabled = true;
        //EditorBuildSettings.scenes = scenes;

        EditorUserBuildSettings.SwitchActiveBuildTarget(targetBuildGroup, targetPlatform);
        Debug.Log("Platform switched to " + targetPlatform);
    }


    public static void CopyFilesRecursively(DirectoryInfo source, DirectoryInfo target)
    {
        foreach (DirectoryInfo dir in source.GetDirectories())
            CopyFilesRecursively(dir, target.CreateSubdirectory(dir.Name));

        foreach (FileInfo file in source.GetFiles())
            file.CopyTo(Path.Combine(target.FullName, file.Name));
    }

    public static void MoveFilesRecursively(DirectoryInfo source, DirectoryInfo target)
    {
        foreach (DirectoryInfo dir in source.GetDirectories())
            MoveFilesRecursively(dir, target.CreateSubdirectory(dir.Name));

        foreach (FileInfo file in source.GetFiles())
            file.MoveTo(Path.Combine(target.FullName, file.Name));
    }

    public static IEnumerable<Scene> OpenScenes()
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
            yield return SceneManager.GetSceneAt(i);
    }
}

#endif