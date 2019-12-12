using UnityEngine;

namespace ECSish
{
    public class EulerAngles : MonoBehaviourComponentData
    {
        public float X
        {
            get => transform.eulerAngles.x;
            set => transform.eulerAngles = new Vector3(value, transform.eulerAngles.y, transform.eulerAngles.z);
        }

        public float Y
        {
            get => transform.eulerAngles.y;
            set => transform.eulerAngles = new Vector3(transform.eulerAngles.x, value, transform.eulerAngles.z);
        }

        public float Z
        {
            get => transform.eulerAngles.z;
            set => transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, value);
        }
    }
}
