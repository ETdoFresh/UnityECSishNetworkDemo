using UnityEngine;

namespace ECSish
{
    public class CollisionEnter : MonoBehaviourComponentData
    {
        private void OnCollisionEnter(Collision collision)
        {
            var collider = collision.collider;
            var force = 0;
            var relativeVelocity = collision.relativeVelocity;
            var impulse = collision.impulse;
            ECSEvent.Create<CollisionEnterEvent>(gameObject, collider, force, relativeVelocity, impulse);
        }
    }
}