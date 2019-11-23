using UnityEngine;

namespace ECSish
{
    public class OnDisableListener : MonoBehaviour
    {
        private void OnDisable()
        {
            EventSystem.Add(() => gameObject.AddComponent<OnDisableEvent>());
        }

    }

    public class OnDisableEvent : MonoBehaviourComponentData { }
}