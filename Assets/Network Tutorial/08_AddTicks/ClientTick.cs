using ECSish;

public class ClientTick : MonoBehaviourComponentData
{
    public int lastReceivedTick;
    public int predictedTick;
}
