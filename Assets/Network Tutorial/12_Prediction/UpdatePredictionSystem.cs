using ECSish;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UpdatePredictionSystem : MonoBehaviourSystem
{
    private List<EntityId> remainingEntityIds = new List<EntityId>();

    private void Update()
    {
        foreach (var entity in GetEntities<OnReceiveEvent, Session>())
        {
            var args = entity.Item1.Args;
            if (args.Length < 1) continue;

            var command = args[0].ToLower();
            if (command == "update")
            {
                var session = entity.Item2;
                var tick = Convert.ToInt32(args[1]);
                foreach (var clientTick in GetEntities<ClientTick>())
                {
                    clientTick.Item1.lastReceivedTick = tick;
                    clientTick.Item1.lastReceivedTime = Time.time;
                }

                var remainingEntityIds = GetCurrentEntityIds();
                var i = 2;
                while (i < args.Length)
                {
                    var entityId = Convert.ToInt32(args[i++]);
                    var prefabName = args[i++];

                    remainingEntityIds.RemoveAll(e => e.entityId == entityId);

                    var clientEntity = GetEntityById(entityId);
                    if (clientEntity == null)
                        clientEntity = CreateEntity(entityId, prefabName);

                    var movementHistory = clientEntity.GetComponent<MovementHistory>();
                    var movementNetworkSync = clientEntity.GetComponent<MovementNetworkSync>();
                    if (movementHistory && movementNetworkSync)
                    {
                        var data = new MovementHistory.Data();
                        data.tick = tick;
                        if (movementNetworkSync.syncPosition)
                        {
                            data.position.x = Convert.ToSingle(args[i++]);
                            data.position.y = Convert.ToSingle(args[i++]);
                            data.position.z = Convert.ToSingle(args[i++]);
                        }
                        if (movementNetworkSync.syncRotation)
                        {
                            data.rotation.x = Convert.ToSingle(args[i++]);
                            data.rotation.y = Convert.ToSingle(args[i++]);
                            data.rotation.z = Convert.ToSingle(args[i++]);
                            data.rotation.w = Convert.ToSingle(args[i++]);
                        }
                        if (movementNetworkSync.syncScale)
                        {
                            data.scale.x = Convert.ToSingle(args[i++]);
                            data.scale.y = Convert.ToSingle(args[i++]);
                            data.scale.z = Convert.ToSingle(args[i++]);
                        }
                        if (movementNetworkSync.syncVelocity)
                        {
                            var velocity = Vector3.zero;
                            data.velocity.x = Convert.ToSingle(args[i++]);
                            data.velocity.y = Convert.ToSingle(args[i++]);
                            data.velocity.z = Convert.ToSingle(args[i++]);
                        }

                        if (movementNetworkSync.syncAngularVelocity)
                        {
                            data.angularVelocity.x = Convert.ToSingle(args[i++]);
                            data.angularVelocity.y = Convert.ToSingle(args[i++]);
                            data.angularVelocity.z = Convert.ToSingle(args[i++]);
                        }
                        movementHistory.Add(data);
                    }
                }

                foreach (var remainingEntity in remainingEntityIds)
                    remainingEntity.gameObject.AddComponent<EntityDestroyed>();
            }
        }
    }

    private List<EntityId> GetCurrentEntityIds()
    {
        remainingEntityIds.Clear();
        foreach (var entity in GetEntities<EntityId>())
            remainingEntityIds.Add(entity.Item1);
        return remainingEntityIds;
    }

    private EntityId GetEntityById(int entityId)
    {
        foreach (var entity in GetEntities<EntityId>())
            if (entity.Item1.entityId == entityId)
                return entity.Item1;

        return null;
    }

    private EntityId CreateEntity(int entityId, string prefabName)
    {
        var prefabList = GetEntity<PrefabList>().Item1.prefabList;
        var prefab = prefabList.prefabs.Where(p => p.name.ToLower() == prefabName.ToLower()).FirstOrDefault();
        var entityGameObject = gameObject.scene.Instantiate(prefab);
        entityGameObject.AddComponent<ClientPrediction>();
        entityGameObject.AddComponent<ClientMovementHistory>();

        entityGameObject.layer = LayerMask.NameToLayer("Client 1");
        for (int i = 0; i < entityGameObject.transform.childCount; i++)
            entityGameObject.transform.GetChild(i).gameObject.layer = LayerMask.NameToLayer("Client 1");

        var entity = entityGameObject.GetComponent<EntityId>();
        entity.entityId = entityId;

        return entity;
    }
}
