using ECSish;
using UnityEngine;

public class ClientTickSystem : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var entity in GetEntities<ClientTick, TickRate>())
        {
            var clientTick = entity.Item1;
            var tickRate = entity.Item2.actual;
            entity.Item1.deltaTime += Time.deltaTime;
            while (entity.Item1.deltaTime >= tickRate)
            {
                clientTick.tick++;
                entity.Item1.deltaTime -= tickRate;
            }
        }
    }
}