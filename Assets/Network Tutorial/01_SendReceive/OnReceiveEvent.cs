using ECSish;

public class OnReceiveEvent : MonoBehaviourComponentData
{
    public string message;
    public string[] Args => message.Split(' ');
}
