namespace QRCode.Framework
{
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UnityEngine.Localization;

    [CreateAssetMenu(menuName = K.Subtitles.SubtitleDataPath)]
    public class SubtitleData : ScriptableObject
    {
        [TitleGroup(K.InspectorGroups.Settings)]
        
        [Title("Speaker Name")]
        [SerializeField] private LocalizedString m_subtitleLocalizedSpeakerName = null;
        [SerializeField] private string m_subtitleSpeakerName;
        
        [Title("Body")]
        [SerializeField] private LocalizedString m_subtitleLocalizedText = null;
        [SerializeField] private string m_subtitleText;
        
        public string GetSubtitleTextForPlaceHolder() => m_subtitleText;
        public string GetSubtitleSpeakerNameForPlaceHolder() => m_subtitleSpeakerName;

        public string GetSpeakerName()
        {
#if UNITY_EDITOR
            if (Application.isPlaying == false)
            {
                return m_subtitleSpeakerName;
            }
#endif
            
            if (!m_subtitleLocalizedSpeakerName.IsEmpty)
            {
                return m_subtitleLocalizedSpeakerName.GetLocalizedString();
            }

            if (!string.IsNullOrEmpty(m_subtitleSpeakerName))
            {
                return m_subtitleSpeakerName;
            }

            return K.Subtitles.SubtitleSpeakerNamePlaceHolder;
        }
        
        public string GetText()
        {
#if UNITY_EDITOR
            if (Application.isPlaying == false)
            {
                return m_subtitleText;
            }
#endif
            
            if (!m_subtitleLocalizedText.IsEmpty)
            {
                return m_subtitleLocalizedText.GetLocalizedString();
            }

            if (!string.IsNullOrEmpty(m_subtitleText))
            {
                return m_subtitleText;
            }

            return K.Subtitles.SubtitleTextPlaceHolder;
        }
    }
}