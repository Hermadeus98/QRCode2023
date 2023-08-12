namespace QRCode.Engine.Toolbox
{
    using Sirenix.OdinInspector;
    using UnityEngine;

    public class PlayAudioSourceInEditor : MonoBehaviour
    {
        [Button]
        private void PlaySound() => GetComponent<AudioSource>().Play();
    }
}
