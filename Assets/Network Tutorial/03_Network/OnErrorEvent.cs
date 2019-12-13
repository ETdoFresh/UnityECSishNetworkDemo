using ECSish;
using System;

public class OnErrorEvent : MonoBehaviourComponentData 
{
    public Exception exception;
    public string Message => exception.Message;
}