﻿using ECSish;
using UnityEngine;

public class ServerTickSystem : MonoBehaviourSystem
{
    private void Update()
    {
        foreach(var entity in GetEntities<ServerTick, TickRate>())
        {
            var serverTick = entity.Item1;
            var tickRate = entity.Item2.actual;
            entity.Item1.deltaTime += Time.deltaTime;
            while (entity.Item1.deltaTime >= tickRate)
            {
                serverTick.tick++;
                entity.Item1.deltaTime -= tickRate;
            }
        }
    }
}
