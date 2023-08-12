namespace QRCode.Engine.Core.Audio
{
    using Engine.Debugging;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UnityEngine.Audio;
    using Toolbox.Extensions;
    using Toolbox;

    [CreateAssetMenu(menuName = AudioConstants.Audio.SoundDataCreateAssetMenuPath, fileName = "New Sound Data")]
    public class SoundData : SerializedScriptableObject
    {
        private struct AudioClips
        {
            [InlineEditor(InlineEditorModes.SmallPreview)]
            [SerializeField] private AudioClip m_audioClip;

            public AudioClip AudioClip => m_audioClip;
        }
        
        [TitleGroup("Generals")][TableList]
        [SerializeField] private AudioClips[] m_audioClips = null;

        [TitleGroup("Generals")] [SerializeField]
        private AudioPlayType m_playType = AudioPlayType.Play;
        [TitleGroup("Generals")] [SerializeField][MinMaxSlider(0f, 1000f)][ShowIf("@this.m_playType == AudioPlayType.PlayDelayed")]
        private Vector2 m_delay = new Vector2(1f, 1f);
        [TitleGroup("Generals")] [SerializeField]
        private AudioMixerGroup m_audioMixerGroup = null;

        [TitleGroup("Settings")] [SerializeField] [Range(0, 256)]
        private int m_priority = 128;
        [TitleGroup("Settings")] [SerializeField]
        private bool m_dontPlayTwiceSameSound = true;
        [TitleGroup("Settings")] [SerializeField] [MinMaxSlider(0f, 1f)]
        private Vector2 m_volume = new Vector2(1f, 1f);
        [TitleGroup("Settings")] [SerializeField] [MinMaxSlider(-3f, 3f)]
        private Vector2 m_pitch = new Vector2(1f, 1f);
        [TitleGroup("Settings")] [SerializeField] [MinMaxSlider(-1f, 1f)]
        private Vector2 m_stereoPan = new Vector2(0f, 0f);
        [TitleGroup("Settings")] [SerializeField] [MinMaxSlider(-3f, 3f)]
        private Vector2 m_spatialBlend = new Vector2(1f, 1f);
        [TitleGroup("Settings")] [SerializeField]
        private bool m_loop = false;
        [TitleGroup("Settings")] [SerializeField]
        private bool m_bypassEffects = false;
        [TitleGroup("Settings")] [SerializeField]
        private bool m_bypassListenerEffects = false;
        [TitleGroup("Settings")] [SerializeField]
        private bool m_bypassReverbZone = false;

        private AudioClip m_lastAudioClipPlayed = null;

        public AudioClip GetAudioClip()
        {
            if (m_audioClips.IsNullOrEmpty())
            {
                QRDebug.DebugError(Constants.DebuggingChannels.Audio, $"Audio Clips should not be null or empty on {name}",
                    this);
                return null;
            }

            var audioClip = m_audioClips.GetRandom().AudioClip;

            if (m_dontPlayTwiceSameSound)
            {
                if (m_audioClips.Length == 1)
                {
                    return m_audioClips[0].AudioClip;
                }
                
                while (audioClip == m_lastAudioClipPlayed)
                {
                    GetAudioClip();
                }
            }
            
            return audioClip;
        }

        public AudioPlayType AudioPlayType => m_playType;
        public AudioMixerGroup AudioMixerGroup => m_audioMixerGroup;
        public int Priority => m_priority;
        public float Volume => Random.Range(m_volume.x, m_volume.y);
        public float Pitch => Random.Range(m_pitch.x, m_pitch.y);
        public float StereoPan => Random.Range(m_stereoPan.x, m_stereoPan.y);
        public float SpatialBlend => Random.Range(m_spatialBlend.x, m_spatialBlend.y);
        public bool Loop => m_loop;
        public bool BypassEffects => m_bypassEffects;
        public bool BypassListenerEffects => m_bypassListenerEffects;
        public bool BypassReverbZone => m_bypassReverbZone;
        public float Delay => Random.Range(m_delay.x, m_delay.y);
        
        protected override void OnBeforeSerialize()
        {
            base.OnBeforeSerialize();
            m_lastAudioClipPlayed = null;
        }
    }

    public enum AudioPlayType
    {
        Play = 0,
        PlayDelayed = 1,
        PlayOneShot = 2,
    }
}
