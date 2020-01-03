using ECSish;
using System;

public class OnReceiveEvent : MonoBehaviourComponentData
{
    public string message;
    public string[] Args => message.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
}
