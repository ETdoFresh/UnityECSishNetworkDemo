using ECSish;
using System.Linq;

public class DestroyTCPClientReceiversIfSessionDoesNotExist : MonoBehaviourSystem
{
    private void Update()
    {
        foreach(var entity in GetEntities<TCPReceivedInput>())
        {
            var session = GetEntities<Session>()
                .Where(s => s.Item1.id == entity.Item1.sessionId)
                .Select(s => s.Item1)
                .FirstOrDefault();

            if (!session)
                entity.Item1.gameObject.AddComponent<EntityDestroyed>();
        }
    }
}