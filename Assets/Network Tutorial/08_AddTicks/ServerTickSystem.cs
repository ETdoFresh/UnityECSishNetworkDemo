using ECSish;
using UnityEngine;

public class ServerTickSystem : MonoBehaviourSystem
{
    private void Update()
    {
        foreach(var entity in GetEntities<ServerUpdateRate, ServerTick>())
        {
            var serverUpdateRate = entity.Item1.updateRateInSeconds;
            var serverTick = entity.Item2;
            serverTick.tick = Mathf.RoundToInt(Time.time / serverUpdateRate);
        }
    }
}
