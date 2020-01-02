using ECSish;
using UnityEngine;

public class SendInputFromSession : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var entity in GetEntities<ClientInputRate, SplitScreenInput>())
        {
            var lastSent = entity.Item1.lastSent;
            var nextSend = lastSent + entity.Item1.sendRateInSeconds;
            if (Time.time < nextSend) continue;

            var input = entity.Item2;
            var sessionEntity = GetEntity<Session>();
            if (sessionEntity == null) continue;

            lastSent = Time.time;
            var message = $"{sessionEntity.Item1.id} Input {input.horizontal} {input.vertical} {input.jumpPress}";
            ECSEvent.Create<OnSendEvent>(sessionEntity.Item1, message);
        }
    }
}