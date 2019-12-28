using ECSish;

public class AssignUnassignedEntityIds : MonoBehaviourSystem
{
    private void Update()
    {
        foreach(var entity in GetEntities<EntityId>())
        {
            var isServer = GetEntity<Server>() != null;
            if (!isServer) continue;

            if (entity.Item1.entityId >= 0) continue;

            entity.Item1.entityId = EntityId.nextEntityId;
            EntityId.nextEntityId++;
        }
    }
}