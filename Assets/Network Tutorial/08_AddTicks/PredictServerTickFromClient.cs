using ECSish;
using UnityEngine;

public class PredictServerTickFromClient : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var tickRateEntity in GetEntities<TargetTickRate>())
            foreach (var entity in GetEntities<Session, SessionTick, RoundTripTime>())
            {
                var tickRate = tickRateEntity.Item1.tickRate;
                var session = entity.Item1;
                var clientTick = entity.Item2;
                var roundTripTime = entity.Item3;

                var timeSinceLastReceived = Time.time - (session.lastReceived - session.lastOffsetReceived);
                var halfRTT = roundTripTime.RTT / 2;
                clientTick.predictedTick = clientTick.lastReceivedTick;
                clientTick.predictedTick += Mathf.RoundToInt((timeSinceLastReceived + halfRTT) / tickRate);
                clientTick.predictedTick += 2; // Arbitrary 2 ticks due to update loop lag, etc...
            }
    }
}
