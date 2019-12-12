using UnityEngine;

namespace ECSish
{
    public class BoxColliderComponent : MonoBehaviourComponentData
    {
        public BoxCollider boxCollider;

        private void OnValidate()
        {
            if (!boxCollider) boxCollider = GetComponent<BoxCollider>();
        }
    }
}