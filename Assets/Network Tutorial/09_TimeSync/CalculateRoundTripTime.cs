using ECSish;

public class CalculateRoundTripTime : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var tickRateEntity in GetEntities<TargetTickRate>())
            foreach (var entity in GetEntities<Session, ClientTick, RoundTripTime>())
            {
                var tickRate = tickRateEntity.Item1.tickRate;
                var session = entity.Item1;
                var clientTick = entity.Item2;
                var roundTripTime = entity.Item3;

                //var sentTime = clientTick.lastReceivedTick * tickRate;
                //var receivedTime = session.lastReceived;
                //var offset = session.lastOffsetReceived;
                //roundTripTime.RTT = receivedTime - (sentTime - offset);
            }
    }
}