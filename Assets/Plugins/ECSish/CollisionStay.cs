using UnityEngine;

namespace ECSish
{
    public class CollisionStay : MonoBehaviourComponentData
    {
        private void OnCollisionStay(Collision collision)
        {
            var collider = collision.collider;
            var relativeVelocity = collision.relativeVelocity;
            var impulse = collision.impulse;
            EventSystem.Add(() => {
                var component = gameObject.AddComponent<CollisionStayEvent>();
                component.collider = collider;
                component.relativeVelocity = relativeVelocity;
                component.impulse = impulse;
                return component;
            });
        }
    }
}
