using ECSish;
using System.Linq;
using UnityEngine;

public class InterpolateMovement : MonoBehaviourSystem
{
    private void Update()
    {
        foreach(var entity in GetEntities<Interpolation, MovementHistory, MovementNetworkSync>())
        {
            var interpolationRate = entity.Item1.interpolationRateInSeconds;
            var movementHistory = entity.Item2.movementHistory;
            var network = entity.Item3;

            var interpolatedTime = Time.time - interpolationRate;
            var before = (MovementHistory.Data)null;
            var after = (MovementHistory.Data)null;
            var t = 0f;

            if (movementHistory.Count == 0)
                continue;

            else if (movementHistory.Count == 1)
                before = after = movementHistory[0];

            else /* if (movementHistory.Count >= 2) */
            {
                var beforeInterpolatedTime = movementHistory.Where(d => d.time < interpolatedTime);
                if (beforeInterpolatedTime.Count() > 0)
                {
                    var maxBeforeTime = beforeInterpolatedTime.Max(d => d.time);
                    before = beforeInterpolatedTime.Where(d => d.time == maxBeforeTime).First();
                }
                else
                {
                    before = movementHistory[0];
                }

                var afterInterpolatedTime = movementHistory.Where(d => d.time >= interpolatedTime);
                if (afterInterpolatedTime.Count() > 0)
                {
                    var minAfterTime = afterInterpolatedTime.Min(d => d.time);
                    after = afterInterpolatedTime.Where(d => d.time == minAfterTime).First();
                }
                else
                {
                    after = movementHistory[movementHistory.Count - 1];
                }
                var deltaTime = after.time - before.time;
                t = deltaTime == 0 ? 0 : (interpolatedTime - before.time) / deltaTime;
            }


            if (network.syncPosition)
                network.transform.position = Vector3.Lerp(before.position, after.position, t);

            if (network.syncRotation)
                network.transform.rotation = Quaternion.Lerp(before.rotation, after.rotation, t);

            if (network.syncScale)
                network.transform.localScale = Vector3.Lerp(before.scale, after.scale, t);

            if (network.syncVelocity)
                network.GetComponent<Rigidbody>().velocity = Vector3.Lerp(before.velocity, after.velocity, t);

            if (network.syncAngularVelocity)
                network.GetComponent<Rigidbody>().angularVelocity = Vector3.Lerp(before.angularVelocity, after.angularVelocity, t);
        }
    }
}