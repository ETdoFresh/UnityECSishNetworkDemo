using UnityEngine;

namespace ECSish
{
    public class OnEnableListener : MonoBehaviour
    {
        private void OnEnable()
        {
            EventSystem.Add(() => gameObject.AddComponent<OnEnableEvent>());
        }

    }

    public class OnEnableEvent : MonoBehaviourComponentData { }
}