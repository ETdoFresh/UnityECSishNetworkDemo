using UnityEngine;

namespace ECSish
{
    public class CollisionExit : MonoBehaviourComponentData
    {
        private void OnCollisionExit(Collision collision)
        {
            var collider = collision.collider;
            var relativeVelocity = collision.relativeVelocity;
            var impulse = collision.impulse;
            ECSEvent.Add(() => {
                var component = gameObject.AddComponent<CollisionExitEvent>();
                component.collider = collider;
                component.relativeVelocity = relativeVelocity;
                component.impulse = impulse;
                return component;
            });
        }
    }
}