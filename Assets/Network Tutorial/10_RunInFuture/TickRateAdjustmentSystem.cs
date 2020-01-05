using ECSish;
using UnityEngine;

public class TickRateAdjustmentSystem : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var rttEntity in GetEntities<RoundTripTime>())
            foreach (var entity in GetEntities<ClientTick, ClientFutureTick, TickRate>())
            {
                var rtt = rttEntity.Item1.RTT;
                var clientTick = entity.Item1;
                var clientFutureTick = entity.Item2;
                var tickRate = entity.Item3;

                var halfRTT = rtt / 2;
                var halfRTTTicks = Mathf.RoundToInt(halfRTT / tickRate.target);
                clientFutureTick.targetTick = clientTick.predictedTick + clientFutureTick.buffer + halfRTTTicks;

                if (clientTick.tick < clientFutureTick.targetTick)
                    tickRate.actual -= clientFutureTick.incrementTickRate;
                else if (clientTick.tick > clientFutureTick.targetTick)
                    tickRate.actual += clientFutureTick.incrementTickRate;
                else /* if (clientTick.tick == clientFutureTick.targetTick) */
                    tickRate.actual = tickRate.target;

                tickRate.actual = Mathf.Max(tickRate.actual, clientFutureTick.minTickRate);
                tickRate.actual = Mathf.Min(tickRate.actual, clientFutureTick.maxTickRate);
            }
    }
}
