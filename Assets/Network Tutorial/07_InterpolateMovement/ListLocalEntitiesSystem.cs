using ECSish;

public class ListLocalEntitiesSystem : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var entity in GetEntities<OnSendEvent>())
        {
            var args = entity.Item1.Args;
            if (args.Length < 1) continue;

            var command = args.Length == 1 ? args[0].ToLower() : args[1].ToLower();
            if (command == "listlocalentities")
            {
                var textMesh = GetEntity<UIConsole>().Item1.textMesh;
                foreach (var localEntity in GetEntities<EntityId>())
                    textMesh.text += $"Entity Id: {localEntity.Item1.entityId} Name: {localEntity.Item1.name}\n";
            }
        }
    }
}