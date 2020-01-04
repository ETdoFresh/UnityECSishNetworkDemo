using ECSish;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UpdateWithTickCommandSystem : MonoBehaviourSystem
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
                    clientTick.Item1.tick = tick;

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

                    var movementNetworkSync = clientEntity.GetComponent<MovementNetworkSync>();
                    if (movementNetworkSync)
                    {
                        if (movementNetworkSync.syncPosition)
                        {
                            var position = Vector3.zero;
                            position.x = Convert.ToSingle(args[i++]);
                            position.y = Convert.ToSingle(args[i++]);
                            position.z = Convert.ToSingle(args[i++]);
                            clientEntity.transform.position = position;
                        }
                        if (movementNetworkSync.syncRotation)
                        {
                            var rotation = Quaternion.identity;
                            rotation.x = Convert.ToSingle(args[i++]);
                            rotation.y = Convert.ToSingle(args[i++]);
                            rotation.z = Convert.ToSingle(args[i++]);
                            rotation.w = Convert.ToSingle(args[i++]);
                            clientEntity.transform.rotation = rotation;
                        }
                        if (movementNetworkSync.syncScale)
                        {
                            var scale = Vector3.zero;
                            scale.x = Convert.ToSingle(args[i++]);
                            scale.y = Convert.ToSingle(args[i++]);
                            scale.z = Convert.ToSingle(args[i++]);
                            clientEntity.transform.localScale = scale;
                        }
                        if (movementNetworkSync.syncVelocity)
                        {
                            var velocity = Vector3.zero;
                            velocity.x = Convert.ToSingle(args[i++]);
                            velocity.y = Convert.ToSingle(args[i++]);
                            velocity.z = Convert.ToSingle(args[i++]);

                            var rigidbody = clientEntity.GetComponent<Rigidbody>();
                            if (rigidbody) rigidbody.velocity = velocity;
                        }

                        if (movementNetworkSync.syncAngularVelocity)
                        {
                            var angularVelocity = Vector3.zero;
                            angularVelocity.x = Convert.ToSingle(args[i++]);
                            angularVelocity.y = Convert.ToSingle(args[i++]);
                            angularVelocity.z = Convert.ToSingle(args[i++]);

                            var rigidbody = clientEntity.GetComponent<Rigidbody>();
                            if (rigidbody) rigidbody.angularVelocity = angularVelocity;
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