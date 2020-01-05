using ECSish;
using UnityEngine;

public class SetClientTickToPredictedTick : MonoBehaviourSystem
{
    private void Update()
    {
        foreach(var entity in GetEntities<ClientTick>())
        {
            var clientTick = entity.Item1;
            var previousTick = clientTick.tick;
            clientTick.tick = Mathf.Max(previousTick, clientTick.predictedTick);
        }
    }
}
