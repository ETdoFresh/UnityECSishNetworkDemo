using UnityEngine;

namespace ECSish
{
    public class OnEnableListener : MonoBehaviour
    {
        private void OnEnable()
        {
            ECSEvent.Create<OnEnableEvent>(gameObject);
        }
    }

    public class OnEnableEvent : MonoBehaviourComponentData { }
}