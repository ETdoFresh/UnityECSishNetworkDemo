using ECSish;
using UnityEngine;

public class SendInputWithTick : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var entity in GetEntities<ClientInputRate, SplitScreenInput, ClientTick>())
            foreach (var sessionEntity in GetEntities<Session>())
            {
                var lastSent = entity.Item1.lastSent;
                var nextSend = lastSent + entity.Item1.sendRateInSeconds;
                var input = entity.Item2;
                var clientTick = entity.Item3;
                var session = sessionEntity.Item1;

                if (Time.time < nextSend) continue;

                entity.Item1.lastSent = lastSent = Time.time;
                var message = $"Input {clientTick.tick} {input.horizontal} {input.vertical} {input.jumpPressed}";
                ECSEvent.Create<OnSendEvent>(session, message);
            }
    }
}
