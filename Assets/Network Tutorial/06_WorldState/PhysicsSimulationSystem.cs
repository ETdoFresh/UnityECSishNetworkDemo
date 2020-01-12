using ECSish;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhysicsSimulationSystem : MonoBehaviourSystem
{
    private float nextUpdate;

    private void OnEnable()
    {
        nextUpdate = Time.time;
    }

    private void Update()
    {
        while (Time.time > nextUpdate)
        {
            nextUpdate += Time.fixedDeltaTime;
            gameObject.scene.GetPhysicsScene().Simulate(Time.fixedDeltaTime);
        }
    }
}