using UnityEngine;

namespace ECSish
{
    public class CollisionEnter : MonoBehaviourComponentData
    {
        private void OnCollisionEnter(Collision collision)
        {
            var collider = collision.collider;
            var relativeVelocity = collision.relativeVelocity;
            var impulse = collision.impulse;
            ECSEvent.Add(() => {
                var component = gameObject.AddComponent<CollisionEnterEvent>();
                component.collider = collider;
                component.relativeVelocity = relativeVelocity;
                component.impulse = impulse;
                return component;
            });
        }
    }
}