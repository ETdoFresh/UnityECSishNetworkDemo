using UnityEngine;

namespace ECSish
{
    public class OnDisableListener : MonoBehaviour
    {
        private void OnDisable()
        {
            ECSEvent.Create<OnDisableEvent>(gameObject);
        }
    }

    public class OnDisableEvent : MonoBehaviourComponentData { }
}