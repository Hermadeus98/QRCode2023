namespace QRCode.Framework
{
    using BOC.BTagged;
    using Sirenix.OdinInspector;
    using UnityEngine;

    [CreateAssetMenu(menuName = K.Audio.SoundDataCreateAssetMenuPath, fileName = "New Sound Data")]
    public class SoundData : BTaggedSO
    {
        private struct AudioClips
        {
            [InlineEditor(InlineEditorModes.SmallPreview)]
            [SerializeField] private AudioClip m_audioClip;
        }
        
        [TitleGroup("Generals")][TableList]
        [SerializeField] private AudioClips[] m_audioClips = null;

        [TitleGroup("Settings")] [SerializeField] [Range(0, 256)]
        private int m_priority = 128;
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
    }
}
