using ECSish;
using System.Linq;

public class ConnectLocalPeerToOtherLocalPeers : MonoBehaviourSystem
{
    private void Update()
    {
        foreach(var entity in GetAllEntities<LocalPeerClient>())
        {
            var self = entity.Item1;
            var others = GetEntities<LocalPeerClient>().Where(e => e.Item1 != self).Select(o => o.Item1).Except(self.otherClients);
            if (others.Count() > 0)
                self.otherClients.AddRange(others);

            self.otherClients.RemoveAll(otherClientExists => !otherClientExists);
        }
    }
}
