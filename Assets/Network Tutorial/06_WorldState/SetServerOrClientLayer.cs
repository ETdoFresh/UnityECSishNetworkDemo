using ECSish;
using UnityEngine;

public class SetServerOrClientLayer : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var entity in GetEntities<TrackServerOrClientLayer>())
        {
            if (entity.Item1.layer != 0) continue;

            entity.Item1.layer = entity.Item1.gameObject.layer;

            var server = GetEntity<Server>();
            if (server != null)
            {
                entity.Item1.layer = LayerMask.NameToLayer("Server");
                ApplyLayer(entity.Item1);
                continue;
            }

            var splitScreenClientNumber = GetEntity<SplitScreenClientNumber>();
            if (splitScreenClientNumber != null)
            {
                if (splitScreenClientNumber.Item1.splitScreenClientNumber == 0)
                    entity.Item1.layer = LayerMask.NameToLayer("Client 1");
                else if (splitScreenClientNumber.Item1.splitScreenClientNumber == 1)
                    entity.Item1.layer = LayerMask.NameToLayer("Client 2");
                else if (splitScreenClientNumber.Item1.splitScreenClientNumber == 2)
                    entity.Item1.layer = LayerMask.NameToLayer("Client 3");

                ApplyLayer(entity.Item1);
            }
        }
    }

    private static void ApplyLayer(TrackServerOrClientLayer entity)
    {
        entity.gameObject.layer = entity.layer;
        ApplyLayerToChildren(entity, entity.transform);
    }

    private static void ApplyLayerToChildren(TrackServerOrClientLayer entity, Transform parent)
    {
        for(int i = 0; i < parent.childCount; i++)
        {
            var child = parent.GetChild(i);
            child.gameObject.layer = entity.layer;
            ApplyLayerToChildren(entity, child);
        }
    }
}