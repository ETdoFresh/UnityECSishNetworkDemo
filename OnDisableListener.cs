using UnityEngine;

namespace ECSish
{
    public class OnDisableListener : MonoBehaviour
    {
        private void OnDisable()
        {
            ECSEvent.Add(() => gameObject.AddComponent<OnDisableEvent>());
        }

    }

    public class OnDisableEvent : MonoBehaviourComponentData { }
}