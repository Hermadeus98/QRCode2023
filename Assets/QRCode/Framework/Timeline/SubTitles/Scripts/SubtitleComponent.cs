namespace QRCode.Framework
{
    using Events;
    using Game;
    using Sirenix.OdinInspector;
    using TMPro;
    using UnityEngine;

    [ExecuteInEditMode]
    public class SubtitleComponent : MonoBehaviour
    {
        [TitleGroup(K.InspectorGroups.References)] 
        [SerializeField] private TextMeshProUGUI m_subtitleSpeakerName = null;
        [SerializeField] private TextMeshProUGUI m_subtitleText = null;
        [SerializeField] private CanvasGroup m_subtitleCanvasGroup = null;

        [TitleGroup(K.InspectorGroups.Settings)] [SerializeField]
        private bool m_isMainSubtitleComponent = true;

        private bool m_showSubtitles = false;

        private UserSettingsData m_userSettingsData = null;
        private UserSettingsData UserSettingsData
        {
            get
            {
                if (m_userSettingsData == null)
                {
                    m_userSettingsData = ServiceLocator.Current.Get<IUserSettingsService>().GetUserSettingsData();
                }

                return m_userSettingsData;
            }
        }

        private void Start()
        {
            if (m_isMainSubtitleComponent)
            {
                Subtitles.Instance.SetMainSubtitleComponent(this);
            }
        }

        private void OnEnable()
        {
            ShowSubtitleEvent.Register(UpdateShowSubtitleFromSettings);

            if (Bootstrap.IsInit())
            {
                UpdateShowSubtitleFromSettings(UserSettingsData.ShowSubtitles);
            }
        }

        private void OnDisable()
        {
            ShowSubtitleEvent.Unregister(UpdateShowSubtitleFromSettings);
        }

        private void Update()
        {
#if UNITY_EDITOR
            if (Application.isPlaying == false)
            {
                if (TextIsValid() == false)
                {
                    m_subtitleCanvasGroup.alpha = 0f;
                }
            }
#endif
        }

        public void SetSpeakerName(string speakerName)
        {
            m_subtitleSpeakerName.SetText(speakerName);
        }

        public void SetTextRaw(string text)
        {
            m_subtitleText.SetText(text);
        }

        public void SetTransparency(float alpha)
        {
            if (m_showSubtitles == false)
            {
                alpha = 0f;
            }
            
            if (TextIsValid() == false)
            {
                alpha = 0f;
            }

            m_subtitleCanvasGroup.alpha = alpha;
        }

        private bool TextIsValid()
        {
            if (string.IsNullOrEmpty(m_subtitleText.text))
            {
                return false;
            }

            return true;
        }

        private void UpdateShowSubtitleFromSettings(bool showSubtitle)
        {
            m_showSubtitles = showSubtitle;
        }
    }
}
