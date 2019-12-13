using UnityEngine;

namespace ECSish
{
    public class CollisionExit : MonoBehaviourComponentData
    {
        private void OnCollisionExit(Collision collision)
        {
            var collider = collision.collider;
            var force = 0;
            var relativeVelocity = collision.relativeVelocity;
            var impulse = collision.impulse;
            ECSEvent.Create<CollisionExitEvent>(gameObject, collider, force, relativeVelocity, impulse);
        }
    }
}