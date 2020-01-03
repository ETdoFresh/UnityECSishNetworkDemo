using ECSish;

public class CalculateRoundTripTime : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var serverUpdateRate in GetEntities<ServerUpdateRate>())
            foreach (var entity in GetEntities<Session, ClientTick, RoundTripTime>())
            {
                var session = entity.Item1;
                var clientTick = entity.Item2;
                var roundTripTime = entity.Item3;

                var sentTime = clientTick.lastReceivedTick * serverUpdateRate.Item1.updateRateInSeconds;
                var receivedTime = session.lastReceived;
                var offset = session.lastOffsetReceived;
                roundTripTime.RTT = receivedTime - sentTime - offset;
            }
    }
}