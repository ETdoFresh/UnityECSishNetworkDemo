using UnityEngine;

namespace ECSish
{
    public class SpriteRendererComponent : MonoBehaviourComponentData
    {
        public SpriteRenderer spriteRenderer;

        private void OnValidate()
        {
            if (!spriteRenderer) spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }
}