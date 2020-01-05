using ECSish;
using UnityEngine;

public class SendMultipleInputs : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var entity in GetEntities<SplitScreenInput, ClientTick, InputBuffer>())
        {
            var input = entity.Item1;
            var clientTick = entity.Item2.tick;
            var inputBuffer = entity.Item3;

            inputBuffer.AddNewInput(clientTick, input.horizontal, input.vertical, input.jumpPress);
        }

        foreach (var entity in GetEntities<ClientInputRate, ClientTick, InputBuffer>())
            foreach (var sessionEntity in GetEntities<Session>())
            {
                var lastSent = entity.Item1.lastSent;
                var nextSend = lastSent + entity.Item1.sendRateInSeconds;
                var clientTick = entity.Item2;
                var inputBuffer = entity.Item3;
                var session = sessionEntity.Item1;

                if (Time.time < nextSend) continue;

                entity.Item1.lastSent = lastSent = Time.time;

                inputBuffer.RemoveTimesOnOrBeforeTick(clientTick.lastReceivedTick);
                if (inputBuffer.inputs.Count > 0)
                {
                    var message = "Input";
                    foreach (var input in inputBuffer.inputs)
                        message += $" {input.tick} {input.horizontal} {input.vertical} {input.jumpPressed}";
                    ECSEvent.Create<OnSendEvent>(session, message);
                }
            }
    }
}
