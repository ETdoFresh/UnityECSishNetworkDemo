using UnityEngine;

namespace ECSish
{
    public class CollisionStay : MonoBehaviourComponentData
    {
        private void OnCollisionStay(Collision collision)
        {
            var collider = collision.collider;
            var force = 0;
            var relativeVelocity = collision.relativeVelocity;
            var impulse = collision.impulse;
            ECSEvent.Create<CollisionStayEvent>(gameObject, collider, force, relativeVelocity, impulse);
        }
    }
}
