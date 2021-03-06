﻿using ECSish;
using System.Linq;
using UnityEngine;

public class SendUpdateFromServerToClients : MonoBehaviourSystem
{
    private void Update()
    {
        var message = "Update";
        foreach (var entity in GetEntities<EntityId, PrefabName>())
        {
            var entityId = entity.Item1.entityId;
            var prefabName = entity.Item2.prefabName;

            message += $" {entityId} {prefabName}";

            var movementNetworkSync = entity.Item1.GetComponent<MovementNetworkSync>();
            if (movementNetworkSync)
            {
                if (movementNetworkSync.syncPosition)
                {
                    var position = movementNetworkSync.transform.position;
                    message += $" {position.x} {position.y} {position.z}";
                }
                if (movementNetworkSync.syncRotation)
                {
                    var rotation = movementNetworkSync.transform.rotation;
                    message += $" {rotation.x} {rotation.y} {rotation.z} {rotation.w}";
                }
                if (movementNetworkSync.syncScale)
                {
                    var scale = movementNetworkSync.transform.localScale;
                    message += $" {scale.x} {scale.y} {scale.z}";
                }
                if (movementNetworkSync.syncVelocity)
                {
                    var velocity = Vector3.zero;
                    if (movementNetworkSync.GetComponent<Rigidbody>())
                        velocity = movementNetworkSync.GetComponent<Rigidbody>().velocity;
                    message += $" {velocity.x} {velocity.y} {velocity.z}";
                }
                if (movementNetworkSync.syncAngularVelocity)
                {
                    var angularVelocity = Vector3.zero;
                    if (movementNetworkSync.GetComponent<Rigidbody>())
                        angularVelocity= movementNetworkSync.GetComponent<Rigidbody>().angularVelocity;
                    message += $" {angularVelocity.x} {angularVelocity.y} {angularVelocity.z}";
                }
            }
        }

        foreach (var entity in GetEntities<ServerUpdateRate>())
        {
            var serverRate = entity.Item1;
            var nextSendTime = serverRate.lastUpdateSent + serverRate.updateRateInSeconds;

            if (Time.time < nextSendTime) continue;

            serverRate.lastUpdateSent = Time.time;

            foreach (var sessionEntity in GetEntities<Session>())
            {
                var session = sessionEntity.Item1;
                ECSEvent.Create<OnSendEvent>(session, message);
            }
        }
    }
}