using ECSish;
using System;

public class SendToOtherLocalPeers : MonoBehaviourSystem
{
    private void Update()
    {
        foreach(var entity in GetAllEntities<OnSendEvent, LocalPeerClient>())
        {
            var message = entity.Item1.message;
            var peer = entity.Item2;
            foreach(var otherEntity in GetEntities<LocalPeerClient>())
            {
                var otherPeer = otherEntity.Item1;
                if (peer == otherPeer) continue;
                ECSEvent.Create<OnReceiveEvent>(otherPeer, message);
            }
        }
    }
}
