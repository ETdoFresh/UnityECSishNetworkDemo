using ECSish;

public class ClientPrediction : MonoBehaviourComponentData 
{
    public float closeSnapDistance = 0.1f;
    public float farSnapDistance = 2f;
    public float tweenSpeed = 0.1f;
    public int sessionId;
    public float nextUpdate;
}