using ECSish;

public class ClientInputRate : MonoBehaviourComponentData
{
    public float lastSent = 0;
    public float sendRateInSeconds = 1;
}
