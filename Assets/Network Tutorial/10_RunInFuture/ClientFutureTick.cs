using ECSish;

public class ClientFutureTick : MonoBehaviourComponentData
{
    public int targetTick;
    public int minBuffer = 2;
    public int buffer = 2;
    public float incrementTickRate = 0.0001f;
    public float minTickRate = 0.015f;
    public float maxTickRate = 0.025f;
    public float error;
}
