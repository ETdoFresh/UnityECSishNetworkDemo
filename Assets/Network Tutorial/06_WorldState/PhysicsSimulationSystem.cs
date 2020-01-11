using ECSish;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhysicsSimulationSystem : MonoBehaviourSystem
{
    private void FixedUpdate()
    {
        gameObject.scene.GetPhysicsScene().Simulate(Time.fixedDeltaTime);
    }
}