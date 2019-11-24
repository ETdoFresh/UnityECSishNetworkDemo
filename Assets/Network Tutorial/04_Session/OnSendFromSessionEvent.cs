using ECSish;

public class OnSendFromSessionEvent : MonoBehaviourComponentData
{
    public int sessionId;
    public string message;
    public string[] args;
}