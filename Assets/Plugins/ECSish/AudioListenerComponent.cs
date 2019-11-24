using UnityEngine;

namespace ECSish
{
    public class AudioListenerComponent : MonoBehaviourComponentData
    {
        public AudioListener audioListener;

        private void OnValidate()
        {
            if (!audioListener) audioListener = GetComponent<AudioListener>();
        }
    }
}