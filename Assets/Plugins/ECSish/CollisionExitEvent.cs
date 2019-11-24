using UnityEngine;

namespace ECSish
{
    public class CollisionExitEvent : MonoBehaviourComponentData
    {
        public new Collider collider;
        public float force;
        internal Vector3 relativeVelocity;
        internal Vector3 impulse;
    }
}