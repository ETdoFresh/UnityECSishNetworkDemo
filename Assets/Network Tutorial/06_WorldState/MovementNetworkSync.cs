using ECSish;

public class MovementNetworkSync : MonoBehaviourComponentData
{
    public bool syncPosition;
    public bool syncRotation;
    public bool syncScale;
    public bool syncVelocity;
    public bool syncAngularVelocity;
}
