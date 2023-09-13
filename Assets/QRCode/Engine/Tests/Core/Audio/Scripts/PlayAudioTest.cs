namespace QRCode.Engine.Core.Audio
{
    using Sirenix.OdinInspector;    
    using UnityEngine;

    public class PlayAudioTest : SerializedMonoBehaviour
    {
        [SerializeField] private SoundData m_soundData = null;

        private IAudioService m_audioService = null;
        
        private void Start()
        {
            //m_audioService = ServiceLocator.Current.Get<IAudioService>();
        }

        [Button]
        private void PlaySound(Transform target)
        {
            m_audioService.PlaySound(m_soundData, target);
        }
    }
}
