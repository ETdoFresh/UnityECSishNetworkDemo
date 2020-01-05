using ECSish;
using System;
using System.Linq;
using UnityEngine;

public class PingPongCommandSystem : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var entity in GetEntities<RoundTripTime, Session>())
        {
            var rtt = entity.Item1;
            var session = entity.Item2;
            if (Time.time < rtt.nextUpdateTime) continue;

            rtt.nextUpdateTime = Time.time + rtt.updateRate;
            foreach (var clientTick in GetEntities<ClientTick>())
            {
                var tick = clientTick.Item1.lastReceivedTick;
                rtt.AddNewTime(tick, Time.time);
                ECSEvent.Create<OnSendEvent>(session, $"Ping {tick}");
            }

            foreach (var serverTick in GetEntities<ServerTick>())
            {
                var tick = serverTick.Item1.tick;
                rtt.AddNewTime(tick, Time.time);
                ECSEvent.Create<OnSendEvent>(session, $"Ping {tick}");
            }
        }

        foreach (var entity in GetEntities<OnReceiveEvent, Session>())
        {
            var args = entity.Item1.Args;
            var session = entity.Item2;
            if (args.Length < 1) continue;

            var command = args[0].ToLower();
            if (command == "ping")
            {
                var tick = Convert.ToInt32(args[1]);
                ECSEvent.Create<OnSendEvent>(session, $"Pong {tick}");
            }
            if (command == "pong")
            {
                var tick = Convert.ToInt32(args[1]);
                foreach (var rttEntity in GetEntities<RoundTripTime>())
                {
                    var rtt = rttEntity.Item1;
                    
                    var rttSendTime = rtt.sendTimes.Where(st => st.tick == tick).FirstOrDefault();
                    if (rttSendTime != null)
                        rtt.RTT = Time.time - rttSendTime.time;

                    rtt.RemoveTimesOnOrBeforeTick(tick);
                }
            }
        }
    }
}
