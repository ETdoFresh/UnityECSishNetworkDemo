using ECSish;

public class HandleRelayClientOnTCPClientDisconnect : MonoBehaviourSystem
{
    private void Update()
    {
        var entity = GetEntity<TCPClient>();
        if (entity == null || !entity.Item1.isConnected)
        {
            foreach (var relayClientEntities in GetEntities<RelayClient>())
            {
                relayClientEntities.Item1.assignedSession = false;
                relayClientEntities.Item1.joinedLobby = false;
                relayClientEntities.Item1.joinedRoom = false;
            }
        }
    }
}