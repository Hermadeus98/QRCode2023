namespace QRCode.Engine.Core.Audio
{
    using Toolbox.Pattern.Pooling;
    using UnityEngine;

    public class AudioService : IAudioService
    {
        private PoolList<AudioHandler> m_audioHandlerPoolList = null;
        private AudioSettings m_settings = null;

        public AudioService()
        {
            m_settings = AudioSettings.Instance;
            m_audioHandlerPoolList = new PoolList<AudioHandler>(m_settings.AudioHandlerPrefab, "Sounds");
        }
        
        public void PlaySound(SoundData soundData)
        {
            GetAudioHandler().PlaySound(soundData);
        }

        public void PlaySound(SoundData soundData, Transform target)
        {
            GetAudioHandler().PlaySound(soundData, target);
        }

        private AudioHandler GetAudioHandler()
        {
            return m_audioHandlerPoolList.Get();
        }
    }
}
