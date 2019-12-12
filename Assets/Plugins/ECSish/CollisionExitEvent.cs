using UnityEngine;

namespace ECSish
{
    public class CollisionExitEvent : MonoBehaviourComponentData
    {
        public new Collider collider;
        public float force;
        public Vector3 relativeVelocity;
        public Vector3 impulse;
    }
}