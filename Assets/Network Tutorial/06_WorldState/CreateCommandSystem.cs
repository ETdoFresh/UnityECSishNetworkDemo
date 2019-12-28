using ECSish;
using System.Linq;
using UnityEngine;

public class CreateCommandSystem : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var entity in GetEntities<OnReceiveEvent, Session>())
        {
            var args = entity.Item1.Args;
            if (args.Length != 2) continue;

            var command = args[0].ToLower();
            if (command == "create")
            {
                var prefabList = GetEntity<PrefabList>();
                if (prefabList == null) continue;

                if (args.Length < 2) continue;

                var prefabName = args[1];
                var prefab = prefabList.Item1.prefabList.prefabs
                    .Where(p => p.name.ToLower() == prefabName.ToLower())
                    .FirstOrDefault();
                if (!prefab) continue;

                var session = entity.Item2;
                var instance = session.gameObject.scene.Instantiate(prefab);
                instance.transform.position = new Vector3(0, 5, 0);
                instance.layer = LayerMask.NameToLayer("Server");

                if (prefabName.ToLower().Contains("player"))
                {
                    var tcpInput = instance.AddComponent<SessionInput>();
                    tcpInput.sessionId = session.id;
                }

                var message = $"{prefab.name} created with EntityId {EntityId.nextEntityId}";
                ECSEvent.Create<OnSendEvent>(session, message);
            }
        }
    }
}
