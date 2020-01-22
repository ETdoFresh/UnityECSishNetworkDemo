using ECSish;
using System.Collections.Generic;

public class ClientJumpQueue : MonoBehaviourComponentData
{
    public Queue<bool> jumpPresses = new Queue<bool>();
}
