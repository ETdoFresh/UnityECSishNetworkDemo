using ECSish;
using UnityEngine;

public class InterpolationTickSystem : MonoBehaviourSystem
{
    private void Update()
    {
        foreach(var entity in GetEntities<InterpolationTick, ClientTick, TickRate>())
        {
            var interpolationTick = entity.Item1;
            var clientTick = entity.Item2;
            var tickRate = entity.Item3;

            interpolationTick.interpolatedTick = clientTick.lastReceivedTick;
            interpolationTick.interpolatedTick += (Time.time - clientTick.lastReceivedTime) / tickRate.actual;
            interpolationTick.interpolatedTick -= interpolationTick.interpolationRate / tickRate.actual;
        }
    }
}