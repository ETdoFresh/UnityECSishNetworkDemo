using ECSish;

public class OnReceiveFromSessionEvent : MonoBehaviourComponentData
{
    public int sessionId;
    public string message;
    public string[] args;
}
