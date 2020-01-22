using ECSish;
using UnityEngine;

public class SendInputFromSession : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var entity in GetEntities<ClientInputRate, SplitScreenInput, ClientJumpQueue>())
        { 
            var lastSent = entity.Item1.lastSent;
            var jumpQueue = entity.Item3.jumpPresses;
            var nextSend = lastSent + entity.Item1.sendRateInSeconds;
            if (Time.time < nextSend) continue;

            var input = entity.Item2;
            var sessionEntity = GetEntity<Session>();
            if (sessionEntity == null) continue;

            lastSent = Time.time;
            var jump = jumpQueue.Count > 0 ? jumpQueue.Dequeue() : false;
            var message = $"{sessionEntity.Item1.id} Input {input.horizontal} {input.vertical} {jump}";
            ECSEvent.Create<OnSendEvent>(sessionEntity.Item1, message);
        }
    }
}