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
                var movementHistory = entity.Item2;
                var history = movementHistory.movementHistory;

                if (history.Count > 0)
                {
                    var data = history[0];
                    if (tick > history[history.Count - 1].tick)
                        data = history[history.Count - 1];
                    else if (tick > history[0].tick)
                        data = GetMovementData(movementHistory, tick);

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

    private MovementHistory.Data GetMovementData(MovementHistory movementHistory, float tick)
    {
        var history = movementHistory.movementHistory;
        var before = history.Where(m => m.tick <= tick).Last();
        if (before != null) movementHistory.ClearBeforeTick(before.tick);

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
