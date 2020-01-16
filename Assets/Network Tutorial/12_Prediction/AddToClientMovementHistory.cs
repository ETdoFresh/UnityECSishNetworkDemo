using ECSish;
using System.Linq;
using UnityEngine;

public class AddToClientMovementHistory : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var clientTickEntity in GetEntities<ClientTick>())
            foreach (var entity in GetEntities<ClientMovementHistory>())
            {
                var clientTick = clientTickEntity.Item1.tick;
                var lastReceivedTick = clientTickEntity.Item1.lastReceivedTick;
                var movementHistory = entity.Item1;
                var current = entity.Item1.transform;

                var data = new MovementHistory.Data();
                data.tick = clientTick;
                data.position = current.position;
                data.rotation = current.rotation;
                data.scale = current.localScale;

                if (current.GetComponent<Rigidbody>())
                {
                    data.velocity = current.GetComponent<Rigidbody>().velocity;
                    data.angularVelocity = current.GetComponent<Rigidbody>().angularVelocity;
                }

                movementHistory.Add(data);
                movementHistory.ClearBeforeTick(lastReceivedTick);
            }

        var recompute = false;
        foreach (var entity in GetEntities<ClientMovementHistory, MovementHistory>())
        {
            var clientMovementHistory = entity.Item1.movementHistory;
            var server = entity.Item2;
            var latestReceived = server.movementHistory[server.movementHistory.Count - 1];
            server.ClearBeforeTick(latestReceived.tick);
            var clientMovement = clientMovementHistory.Where(d => d.tick == latestReceived.tick).FirstOrDefault();
            if (clientMovement == null)
                continue;

            var acceptableError = entity.Item1.acceptableError;
            var error = Vector3.Distance(latestReceived.position, clientMovement.position);
            if (error > acceptableError)
            {
                recompute = true;
                break;
            }
        }

        if (recompute)
        {
            Debug.Log("Recompute!");
            foreach (var clientTickEntity in GetEntities<ClientTick, InputBuffer>())
            {
                var clientTick = clientTickEntity.Item1.tick;
                var inputBuffer = clientTickEntity.Item2.inputs;
                var latestReceived = clientTickEntity.Item1.lastReceivedTick;
                foreach (var entity in GetEntities<ClientMovementHistory, MovementHistory>())
                {
                    var current = entity.Item1.transform;
                    var clientMovementHistory = entity.Item1.movementHistory;
                    var serverMovementHistory = entity.Item2.movementHistory;
                    clientMovementHistory.Clear();
                    var corrected = serverMovementHistory.Where(d => d.tick == latestReceived).FirstOrDefault();
                    if (corrected != null)
                    {
                        clientMovementHistory.Add(corrected);
                        current.position = corrected.position;
                        current.rotation = corrected.rotation;
                        current.localScale = corrected.scale;
                        if (current.GetComponent<Rigidbody>())
                        {
                            current.GetComponent<Rigidbody>().velocity = corrected.velocity;
                            current.GetComponent<Rigidbody>().angularVelocity = corrected.angularVelocity;
                        }
                    }
                }

                for (var tick = latestReceived + 1; tick <= clientTick; tick++)
                {
                    var input = inputBuffer.Where(i => i.tick == tick).FirstOrDefault();
                    if (input != null)
                        foreach (var sessionEntity in GetEntities<Session>())
                            foreach (var entity in GetEntities<ClientPrediction, Movement, RigidbodyComponent>())
                            {
                                var sessionId = sessionEntity.Item1.id;
                                var clientPrediction = entity.Item1;
                                var movement = entity.Item2;
                                var rigidbody = entity.Item3.rigidbody;

                                if (clientPrediction.sessionId != sessionId)
                                    continue;

                                var forward = Camera.main.transform.forward;
                                var right = Camera.main.transform.right;
                                forward.y = 0f;
                                right.y = 0f;
                                forward.Normalize();
                                right.Normalize();

                                var inputForce = forward * input.vertical;
                                inputForce += right * input.horizontal;
                                var jump = input.jumpPressed;

                                rigidbody.AddForce(inputForce * movement.force);
                                if (jump)
                                    rigidbody.AddForce(Vector3.up * movement.force, ForceMode.Impulse);
                            }

                    gameObject.scene.GetPhysicsScene().Simulate(Time.fixedDeltaTime);

                    foreach (var entity in GetEntities<ClientMovementHistory, RigidbodyComponent>())
                    {
                        var movementHistory = entity.Item1;
                        var current = entity.Item1.transform;
                        var rigidbody = entity.Item2.rigidbody;
                        var data = new MovementHistory.Data();
                        data.tick = tick;
                        data.position = current.position;
                        data.rotation = current.rotation;
                        data.scale = current.localScale;

                        if (rigidbody)
                        {
                            data.velocity = rigidbody.velocity;
                            data.angularVelocity = rigidbody.angularVelocity;
                        }

                        movementHistory.Add(data);
                    }
                }
            }
        }
    }
}
