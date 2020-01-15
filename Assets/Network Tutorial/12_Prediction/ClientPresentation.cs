using ECSish;
using UnityEngine;

public class ClientPresentation : MonoBehaviourComponentData 
{
    public Transform target;
    public float closeSnapDistance = 0.1f;
    public float farSnapDistance = 6f;
    public float tweenSpeed = 0.1f;
}
