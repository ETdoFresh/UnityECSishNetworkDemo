using ECSish;
using UnityEngine;

public class SendInputWithTick : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var entity in GetEntities<ClientInputRate, SplitScreenInput, JumpQueue>())
            foreach (var sessionEntity in GetEntities<Session, SessionTick>())
            {
                var lastSent = entity.Item1.lastSent;
                var nextSend = lastSent + entity.Item1.sendRateInSeconds;
                var input = entity.Item2;
                var jumpQueue = entity.Item3.jumpQueue;
                var session = sessionEntity.Item1;
                var clientTick = sessionEntity.Item2;

                if (Time.time < nextSend) continue;

                var jumpPress = jumpQueue.Count > 0 ? jumpQueue.Dequeue() : false;
                entity.Item1.lastSent = lastSent = Time.time;
                var message = $"Input {clientTick.lastReceivedTick} {clientTick.predictedTick} {session.lastReceived} {input.horizontal} {input.vertical} {jumpPress}";
                ECSEvent.Create<OnSendEvent>(session, message);
            }
    }
}
