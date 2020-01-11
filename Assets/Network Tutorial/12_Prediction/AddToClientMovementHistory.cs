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

            var error = Vector3.Distance(latestReceived.position, clientMovement.position);
            if (error > 0.5f)
            {
                recompute = true;
                break;
            }
        }

        if (recompute)
        {
            Debug.Log("Recompute!");
            foreach (var clientTickEntity in GetEntities<ClientTick>())
            {
                var clientTick = clientTickEntity.Item1.tick;
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
                    gameObject.scene.GetPhysicsScene().Simulate(Time.fixedDeltaTime);
                    foreach (var entity in GetEntities<ClientMovementHistory>())
                    {
                        var movementHistory = entity.Item1;
                        var current = entity.Item1.transform;
                        var data = new MovementHistory.Data();
                        data.tick = tick;
                        data.position = current.position;
                        data.rotation = current.rotation;
                        data.scale = current.localScale;

                        if (current.GetComponent<Rigidbody>())
                        {
                            data.velocity = current.GetComponent<Rigidbody>().velocity;
                            data.angularVelocity = current.GetComponent<Rigidbody>().angularVelocity;
                        }

                        movementHistory.Add(data);
                    }
                }
            }
        }
    }
}
