using UnityEngine;

namespace ECSish
{
    public class RigidbodyComponent : MonoBehaviourComponentData
    {
        public new Rigidbody rigidbody;

        private void OnValidate()
        {
            if (!rigidbody) rigidbody = GetComponent<Rigidbody>();
        }
    }
}