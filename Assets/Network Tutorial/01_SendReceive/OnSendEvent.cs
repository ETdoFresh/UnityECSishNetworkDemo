using ECSish;

public class OnSendEvent : MonoBehaviourComponentData
{
    public string message;
    public string[] Args => message.Split(' ');
}
