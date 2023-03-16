namespace QRCode.Framework
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Gameplay.Pooling;
    using Sirenix.OdinInspector;
    using UnityEngine;

    public class AudioHandler : SerializedMonoBehaviour, IPoolObject
    {
        [TitleGroup(K.InspectorGroups.References)] 
        [SerializeField][Required] private AudioSource m_audioSource = null;

        private SoundData m_soundData = null;
        private Transform m_target = null;
        private CancellationTokenSource m_cancellationTokenSource = null;
        
        public bool IsAvailable { get; set; }

        private void Start()
        {
            m_cancellationTokenSource = new CancellationTokenSource();
        }

        private void OnDestroy()
        {
            m_cancellationTokenSource?.Cancel();
        }

        public void PlaySound(SoundData soundData, Transform target = null)
        {
            m_soundData = soundData;
            m_target = target;
            
            var audioClip = soundData.GetAudioClip();
            m_audioSource.clip = audioClip;
            m_audioSource.outputAudioMixerGroup = soundData.AudioMixerGroup;
            var volume = soundData.Volume;
            m_audioSource.volume = volume;
            m_audioSource.pitch = soundData.Pitch;
            m_audioSource.priority = soundData.Priority;
            m_audioSource.panStereo = soundData.StereoPan;
            m_audioSource.spatialBlend = soundData.SpatialBlend;
            m_audioSource.loop = soundData.Loop;
            m_audioSource.bypassEffects = soundData.BypassEffects;
            m_audioSource.bypassListenerEffects = soundData.BypassListenerEffects;
            m_audioSource.bypassReverbZones = soundData.BypassReverbZone;

            switch (soundData.AudioPlayType)
            {
                case AudioPlayType.Play:
                    m_audioSource.Play();
                    StopSoundDelayed(audioClip.length);
                    break;
                case AudioPlayType.PlayDelayed:
                    var delay = soundData.Delay;
                    m_audioSource.PlayDelayed(delay);
                    StopSoundDelayed(audioClip.length + delay);
                    break;
                case AudioPlayType.PlayOneShot:
                    m_audioSource.PlayOneShot(audioClip, volume);
                    StopSoundDelayed(audioClip.length);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void StopSound()
        {
            m_audioSource.Stop();
        }

        private async void StopSoundDelayed(float delay)
        {
            if (m_cancellationTokenSource == null)
            {
                m_cancellationTokenSource = new CancellationTokenSource();
            }
            
            await Task.Delay(TimeSpan.FromSeconds(delay), m_cancellationTokenSource.Token);
            PoolSystemUtility.Push(this);
        }
        
        private void Update()
        {
            if (m_target != null)
            {
                transform.position = m_target.transform.position;
            }
        }

        public void OnPool()
        {
            
        }

        public void OnPush()
        {
            
        }

        private void OnDrawGizmos()
        {
            if (m_target != null && m_soundData != null)
            {
                var alpha = Mathf.InverseLerp(m_audioSource.spatialBlend, .5f, 1f);
                
                Gizmos.color = new Color(0.95f, 0.77f, 0.06f, alpha);
                Gizmos.DrawWireSphere(m_target.position, m_audioSource.minDistance);
                Gizmos.color = new Color(0.95f, 0.61f, 0.07f, alpha);
                Gizmos.DrawWireSphere(m_target.position, m_audioSource.maxDistance);
            }
        }
    }
}
