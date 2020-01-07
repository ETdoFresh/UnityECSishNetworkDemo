using ECSish;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InterpolateObjectSystem : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var interpolationEntity in GetEntities<InterpolationTick>())
            foreach (var entity in GetEntities<MovementNetworkSync, MovementHistory>())
            {
                var tick = interpolationEntity.Item1.interpolatedTick;
                var sync = entity.Item1;
                var history = entity.Item2.movementHistory;

                if (history.Count > 0)
                {
                    var data = history[0];
                    if (tick > history[history.Count - 1].tick)
                        data = history[history.Count - 1];
                    else if (tick > history[0].tick)
                        data = GetMovementData(history, tick);

                    if (sync.syncPosition) sync.transform.position = data.position;
                    if (sync.syncRotation) sync.transform.rotation = data.rotation;
                    if (sync.syncScale) sync.transform.localScale = data.scale;

                    var rigidbody = sync.GetComponent<Rigidbody>();
                    if (rigidbody)
                    {
                        if (sync.syncVelocity) rigidbody.velocity = data.velocity;
                        if (sync.syncAngularVelocity) rigidbody.angularVelocity = data.angularVelocity;
                    }
                }
            }
    }

    private MovementHistory.Data GetMovementData(List<MovementHistory.Data> history, float tick)
    {
        var before = history.Where(m => m.tick <= tick).Last();
        var after = history.Where(m => m.tick >= tick).First();
        var delta = after.tick - before.tick;
        if (delta == 0) return before;

        var numerator = tick - before.tick;
        var t = numerator / delta;
        var data = new MovementHistory.Data();
        data.position = Vector3.Lerp(before.position, after.position, t);
        data.rotation = Quaternion.Lerp(before.rotation, after.rotation, t);
        data.scale = Vector3.Lerp(before.scale, after.scale, t);
        data.velocity = Vector3.Lerp(before.velocity, after.velocity, t);
        data.angularVelocity = Vector3.Lerp(before.angularVelocity, after.angularVelocity, t);
        return data;
    }
}
