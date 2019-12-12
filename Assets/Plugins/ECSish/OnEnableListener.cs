using UnityEngine;

namespace ECSish
{
    public class OnEnableListener : MonoBehaviour
    {
        private void OnEnable()
        {
            ECSEvent.Add(() => gameObject.AddComponent<OnEnableEvent>());
        }

    }

    public class OnEnableEvent : MonoBehaviourComponentData { }
}