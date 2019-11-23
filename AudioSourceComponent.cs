using UnityEngine;

namespace ECSish
{
    public class AudioSourceComponent : MonoBehaviourComponentData
    {
        public new AudioSource audio;

        private void OnValidate()
        {
            if (!audio) audio = GetComponent<AudioSource>();
        }
    }
}