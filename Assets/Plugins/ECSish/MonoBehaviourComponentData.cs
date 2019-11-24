using UnityEngine;

namespace ECSish
{
    public class MonoBehaviourComponentData : MonoBehaviour
    {
        private void OnEnable() => Entity.Add(this);
        private void OnDisable() => Entity.Remove(this);
    }
}