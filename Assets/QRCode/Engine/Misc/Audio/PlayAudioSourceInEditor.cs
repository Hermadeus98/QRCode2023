using UnityEngine;

namespace QRCode.Framework
{
    using Sirenix.OdinInspector;

    public class PlayAudioSourceInEditor : MonoBehaviour
    {
        [Button]
        private void PlaySound() => GetComponent<AudioSource>().Play();
    }
}
