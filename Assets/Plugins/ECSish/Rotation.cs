using UnityEngine;

namespace ECSish
{
    public class Rotation : MonoBehaviourComponentData
    {
        public float X
        {
            get => transform.rotation.x;
            set => transform.rotation = new Quaternion(value, transform.rotation.y, transform.rotation.z, transform.rotation.w);
        }

        public float Y
        {
            get => transform.rotation.y;
            set => transform.rotation = new Quaternion(transform.rotation.x, value, transform.rotation.z, transform.rotation.w);
        }

        public float Z
        {
            get => transform.rotation.z;
            set => transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, value, transform.rotation.w);
        }

        public float W
        {
            get => transform.rotation.w;
            set => transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, value);
        }
    }
}