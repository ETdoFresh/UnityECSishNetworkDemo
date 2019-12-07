using ECSish;
using System.Linq;
using UnityEngine;

public class CreateCommandSystem : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var entity in GetEntities<OnReceiveFromSessionEvent, SocketClientConnection>())
        {
            var args = entity.Item1.args;
            if (args.Length < 2) continue;

            var command = args[1].ToLower();
            if (command == "create")
            {
                var prefabList = GetEntity<PrefabList>();
                if (prefabList == null) continue;

                if (args.Length < 3) continue;

                var prefabName = args[2];
                var prefab = prefabList.Item1.prefabList.prefabs
                    .Where(p => p.name.ToLower() == prefabName.ToLower())
                    .FirstOrDefault();
                if (!prefab) continue;

                var client = entity.Item2;
                var instance = client.gameObject.scene.Instantiate(prefab);
                instance.transform.position = new Vector3(0, 5, 0);
                instance.layer = LayerMask.NameToLayer("Server");

                if (prefabName.ToLower().Contains("player"))
                {
                    var sessionId = entity.Item1.sessionId;
                    var tcpInput = instance.AddComponent<TCPReceivedInput>();
                    tcpInput.sessionId = sessionId;
                }

                var message = $"{prefab.name} created with EntityId {EntityId.nextEntityId}";
                EventUtility.CreateOnSendEvent(client.gameObject, message);
            }
        }
    }
}
