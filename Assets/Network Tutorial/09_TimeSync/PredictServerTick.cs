using ECSish;
using UnityEngine;

public class PredictServerTick : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var rttEntity in GetEntities<RoundTripTime>())
            foreach (var tickEntity in GetEntities<ClientTick, TickRate>())
            {
                var rtt = rttEntity.Item1;
                var clientTick = tickEntity.Item1;
                var targetTickRate = tickEntity.Item2.target;

                clientTick.predictedTick = clientTick.lastReceivedTick;
                var timeSinceLastUpdated = Time.time - clientTick.lastReceivedTime;
                var halfRTT = rtt.RTT / 2;
                var timeSinceSentTick = timeSinceLastUpdated + halfRTT;
                var ticksSinceSentTick = timeSinceSentTick / targetTickRate;
                var roundedTicksSinceSentTick = Mathf.RoundToInt(ticksSinceSentTick);
                clientTick.predictedTick += roundedTicksSinceSentTick;
                clientTick.predictedTick += 1; // Arbitrary extra tick (I think to do with running all events on the main thread)
            }
    }
}
