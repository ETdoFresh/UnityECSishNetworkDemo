using ECSish;
using UnityEngine;

public class GetApplicationInformationSystem : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var entity in GetEntities<ApplicationInformation>())
        {
            entity.Item1.fps = Time.timeScale / Time.deltaTime;

            if (string.IsNullOrEmpty(entity.Item1.platform))
                entity.Item1.platform = Application.platform.ToString();

            if (string.IsNullOrEmpty(entity.Item1.version))
                entity.Item1.version = Application.version;

            if (string.IsNullOrEmpty(entity.Item1.companyName))
                entity.Item1.companyName = Application.companyName;

            if (string.IsNullOrEmpty(entity.Item1.productName))
                entity.Item1.productName = Application.productName;

            if (string.IsNullOrEmpty(entity.Item1.unityVersion))
                entity.Item1.unityVersion = Application.unityVersion;

            if (entity.Item1.targetFrameRate == 0 && Application.targetFrameRate != 0)
                entity.Item1.targetFrameRate = Application.targetFrameRate;
            else if (entity.Item1.targetFrameRate != Application.targetFrameRate)
                Application.targetFrameRate = entity.Item1.targetFrameRate;

            if (entity.Item1.vSyncCount == -1 && QualitySettings.vSyncCount != -1)
                entity.Item1.vSyncCount = QualitySettings.vSyncCount;
            else if (entity.Item1.vSyncCount != QualitySettings.vSyncCount)
                QualitySettings.vSyncCount = entity.Item1.vSyncCount;
        }
    }
}