using ECSish;
using UnityEngine;

public class PhysicsSimulationSystem : MonoBehaviourSystem
{
    private void FixedUpdate()
    {
        Physics.Simulate(Time.fixedDeltaTime);
    }
}