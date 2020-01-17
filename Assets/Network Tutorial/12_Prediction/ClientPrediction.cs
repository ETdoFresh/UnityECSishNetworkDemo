using ECSish;
using UnityEngine;

public class ClientPrediction : MonoBehaviourComponentData 
{
    public GameObject prefab;
    public ClientPresentation presentation;
    public float closeSnapDistance = 0.01f;
    public float farSnapDistance = 1.5f;
    public float tweenSpeed = 0.1f;
    public int sessionId;
    public float nextUpdate;
}