using ECSish;
using System.Linq;
using UnityEngine;

public class SendMultipleInputs : MonoBehaviourSystem
{
    private void Update()
    {
        var clientPrediction = GetEntity<ClientPrediction>();

        foreach (var entity in GetEntities<SplitScreenInput, ClientTick, InputBuffer>())
        {
            var input = entity.Item1;
            var clientTick = entity.Item2.tick;
            var inputBuffer = entity.Item3;

            var jump = input.jumpPressed;
            if (clientPrediction != null && clientPrediction.Item1.jumpPresses.Count > 0)
                jump = clientPrediction.Item1.jumpPresses.Dequeue();

            if (inputBuffer.inputs.Count > 0)
            {
                while (inputBuffer.inputs.Last().tick + 1 < clientTick)
                    inputBuffer.AddNewInput(inputBuffer.inputs.Last().tick + 1, input.horizontal, input.vertical, false);

                inputBuffer.AddNewInput(clientTick, input.horizontal, input.vertical, jump);
            }
            else
                inputBuffer.AddNewInput(clientTick, input.horizontal, input.vertical, jump);
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
