using ECSish;
using UnityEngine;

public class ServerTickSystem : MonoBehaviourSystem
{
    private void Update()
    {
        foreach(var entity in GetEntities<ServerTick, TargetTickRate>())
        {
            var serverTick = entity.Item1;
            var tickRate = entity.Item2.tickRate;
            serverTick.tick = Mathf.FloorToInt(Time.time / tickRate);
        }
    }
}
