using ECSish;

public class ServerUpdateRate : MonoBehaviourComponentData
{
    public float lastUpdateSent;
    public float updateRateInSeconds;
}