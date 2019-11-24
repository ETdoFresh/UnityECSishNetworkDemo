using ECSish;

public class RelayClient : MonoBehaviourComponentData
{
    public string lobby;
    public string room;
    public bool assignedSession;
    public bool joinedLobby;
    public bool joinedRoom;
}