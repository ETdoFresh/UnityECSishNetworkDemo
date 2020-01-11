using ECSish;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static MovementHistory;

public class ClientMovementHistory : MonoBehaviourComponentData
{
    public int maxRecords = 100;
    public List<Data> movementHistory = new List<Data>();

    public void Add(int tick, Vector3 position, Quaternion rotation, Vector3 scale)
    {
        if (!movementHistory.Where(m => m.tick == tick).Any())
            movementHistory.Add(new Data { tick = tick, position = position, rotation = rotation, scale = scale });
    }

    public void Add(Data data)
    {
        if (!movementHistory.Where(m => m.tick == data.tick).Any())
            movementHistory.Add(data);
    }

    public void ClearBeforeTick(int tick)
    {
        for (int i = movementHistory.Count - 1; i >= 0; i--)
            if (movementHistory[i].tick < tick)
                movementHistory.RemoveAt(i);
    }

    public void ClearAfterTick(int tick)
    {
        for (int i = movementHistory.Count - 1; i >= 0; i--)
            if (movementHistory[i].tick > tick)
                movementHistory.RemoveAt(i);
    }
}
