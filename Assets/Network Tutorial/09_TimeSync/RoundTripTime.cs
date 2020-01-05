using ECSish;
using System;
using System.Collections.Generic;

public class RoundTripTime : MonoBehaviourComponentData
{
    public float RTT;
    public float updateRate = 0.5f;
    internal float nextUpdateTime;
    public List<TickOffset> sendTimes = new List<TickOffset>();

    public void AddNewTime(int tick, float time)
    {
        sendTimes.Add(new TickOffset { tick = tick, time = time });
    }

    public void RemoveTimesOnOrBeforeTick(int tick)
    {
        for (int i = sendTimes.Count - 1; i >= 0; i--)
            if (sendTimes[i].tick <= tick)
                sendTimes.RemoveAt(i);
    }

    [Serializable]
    public class TickOffset
    {
        public int tick;
        public float time;
    }
}
