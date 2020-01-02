using ECSish;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UpdateInterpolatedCommandSystem : MonoBehaviourSystem
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
                var remainingEntityIds = GetCurrentEntityIds();
                var i = 1;
                while (i < args.Length)
                {
                    var entityId = Convert.ToInt32(args[i++]);
                    var prefabName = args[i++];

                    remainingEntityIds.RemoveAll(e => e.entityId == entityId);

                    var clientEntity = GetEntityById(entityId);
                    if (clientEntity == null)
                        clientEntity = CreateEntity(entityId, prefabName);

                    var movementNetworkSync = clientEntity.GetComponent<MovementNetworkSync>();
                    if (movementNetworkSync)
                    {
                        var data = new MovementHistory.Data();
                        data.time = Time.time;

                        if (movementNetworkSync.syncPosition)
                        {
                            var position = Vector3.zero;
                            position.x = Convert.ToSingle(args[i++]);
                            position.y = Convert.ToSingle(args[i++]);
                            position.z = Convert.ToSingle(args[i++]);
                            data.position = position;
                        }
                        if (movementNetworkSync.syncRotation)
                        {
                            var rotation = Quaternion.identity;
                            rotation.x = Convert.ToSingle(args[i++]);
                            rotation.y = Convert.ToSingle(args[i++]);
                            rotation.z = Convert.ToSingle(args[i++]);
                            rotation.w = Convert.ToSingle(args[i++]);
                            data.rotation = rotation;
                        }
                        if (movementNetworkSync.syncScale)
                        {
                            var scale = Vector3.zero;
                            scale.x = Convert.ToSingle(args[i++]);
                            scale.y = Convert.ToSingle(args[i++]);
                            scale.z = Convert.ToSingle(args[i++]);
                            data.scale = scale;
                        }
                        if (movementNetworkSync.syncVelocity)
                        {
                            var velocity = Vector3.zero;
                            velocity.x = Convert.ToSingle(args[i++]);
                            velocity.y = Convert.ToSingle(args[i++]);
                            velocity.z = Convert.ToSingle(args[i++]);
                            data.velocity = velocity;
                        }

                        if (movementNetworkSync.syncAngularVelocity)
                        {
                            var angularVelocity = Vector3.zero;
                            angularVelocity.x = Convert.ToSingle(args[i++]);
                            angularVelocity.y = Convert.ToSingle(args[i++]);
                            angularVelocity.z = Convert.ToSingle(args[i++]);
                            data.angularVelocity = angularVelocity;
                        }

                        var movementHistory = clientEntity.GetComponent<MovementHistory>();
                        if (movementHistory)
                        {
                            if (movementHistory.movementHistory.Count == 0)
                            {
                                if (movementNetworkSync.syncPosition)
                                    movementHistory.transform.position = data.position;
                                if (movementNetworkSync.syncRotation)
                                    movementHistory.transform.rotation = data.rotation;
                                if (movementNetworkSync.syncScale)
                                    movementHistory.transform.localScale = data.scale;
                                if (movementNetworkSync.syncVelocity)
                                    movementHistory.GetComponent<Rigidbody>().velocity = data.velocity;
                                if (movementNetworkSync.syncAngularVelocity)
                                    movementHistory.GetComponent<Rigidbody>().angularVelocity = data.angularVelocity;
                            }
                            movementHistory.movementHistory.Add(data);

                            while (movementHistory.movementHistory.Count > movementHistory.maxRecords)
                                movementHistory.movementHistory.RemoveAt(0);
                        }
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
        entityGameObject.AddComponent<Interpolation>().interpolationRateInSeconds = 0.2f;

        entityGameObject.layer = LayerMask.NameToLayer("Client 1");
        for (int i = 0; i < entityGameObject.transform.childCount; i++)
            entityGameObject.transform.GetChild(i).gameObject.layer = LayerMask.NameToLayer("Client 1");

        var entity = entityGameObject.GetComponent<EntityId>();
        entity.entityId = entityId;

        // TODO: Figure out a better way to handle dummy client objects
        Destroy(entity.GetComponent<Rigidbody>());
        Destroy(entity.GetComponent<Collider>());

        return entity;
    }
}
