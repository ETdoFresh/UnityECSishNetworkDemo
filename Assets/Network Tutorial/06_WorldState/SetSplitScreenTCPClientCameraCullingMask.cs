using ECSish;
using System.Linq;
using UnityEngine;

public class SetSplitScreenTCPClientCameraCullingMask : MonoBehaviourSystem
{
    private int showOnlyClient1Layer;
    private int showOnlyClient2Layer;
    private int showOnlyClient3Layer;

    private void Awake()
    {
        var serverLayer = 1 << LayerMask.NameToLayer("Server");
        var client1Layer = 1 << LayerMask.NameToLayer("Client 1");
        var client2Layer = 1 << LayerMask.NameToLayer("Client 2");
        var client3Layer = 1 << LayerMask.NameToLayer("Client 3");
        showOnlyClient1Layer = ~serverLayer & ~client2Layer & ~client3Layer;
        showOnlyClient2Layer = ~serverLayer & ~client1Layer & ~client3Layer;
        showOnlyClient3Layer = ~serverLayer & ~client1Layer & ~client2Layer;
    }

    private void Update()
    {
        foreach (var entity in GetEntities<TrackTCPClientCameraCullingMask>())
        {
            var splitScreenClientNumber = GetEntity<SplitScreenClientNumber>();
            if (splitScreenClientNumber == null) continue;

            var camera = entity.Item1.GetComponent<Camera>();
            if (!camera) continue;

            if (splitScreenClientNumber.Item1.splitScreenClientNumber == 0)
                camera.cullingMask = showOnlyClient1Layer;
            else if (splitScreenClientNumber.Item1.splitScreenClientNumber == 1)
                camera.cullingMask = showOnlyClient2Layer;
            else if (splitScreenClientNumber.Item1.splitScreenClientNumber == 2)
                camera.cullingMask = showOnlyClient3Layer;
        }
    }
}
