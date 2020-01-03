using ECSish;
using System.Collections.Generic;

public class JumpQueue : MonoBehaviourComponentData
{
    public Queue<bool> jumpQueue = new Queue<bool>();
}
