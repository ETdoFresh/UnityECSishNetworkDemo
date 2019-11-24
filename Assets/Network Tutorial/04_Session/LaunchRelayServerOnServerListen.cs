using ECSish;
using System.Collections.Generic;
using System.Diagnostics;

public class LaunchRelayServerOnServerListen : MonoBehaviourSystem
{
    public List<Process> processes = new List<Process>();

    private void Update()
    {
#if UNITY_EDITOR
        foreach (var entity in GetEntities<OnListeningEvent>())
        {
            var startInfo = new ProcessStartInfo();
            startInfo.FileName = @"NetworkHub\NetworkHub\bin\Debug\NetworkHub.exe";
            var process = Process.Start(startInfo);
            processes.Add(process);
        }
#endif
    }

    private void OnDestroy()
    {
#if UNITY_EDITOR
        foreach (var process in processes)
            if (!process.HasExited)
                process.Kill();
#endif
    }
}