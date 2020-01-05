using ECSish;

public class ClientTick : MonoBehaviourComponentData
{
    public int tick;
    public float deltaTime;
    public int lastReceivedTick;
    public float lastReceivedTime;
    public int predictedTick;
}
