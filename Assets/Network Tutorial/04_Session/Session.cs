using ECSish;

public class Session : MonoBehaviourComponentData
{
    public static int nextId;

    public int id;
    public string ip;
    public int port;
    public string connectionType;
    public string build;
    public string nickname;
    public int connectionId = -1;
}
