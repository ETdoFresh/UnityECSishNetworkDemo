using UnityEngine;

namespace ECSish
{
    public class Translation : MonoBehaviourComponentData
    {
        public float X
        {
            get => transform.position.x;
            set => transform.position = new Vector3(value, transform.position.y, transform.position.z);
        }

        public float Y
        {
            get => transform.position.y;
            set => transform.position = new Vector3(transform.position.x, value, transform.position.z);
        }

        public float Z
        {
            get => transform.position.z;
            set => transform.position = new Vector3(transform.position.x, transform.position.y, value);
        }
    }
}