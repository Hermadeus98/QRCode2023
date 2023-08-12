namespace QRCode.Engine.Game.Subtitles
{
    using Core.UserSettings;
    using Core.UserSettings.Events.SoundSettings;
    using Toolbox;
    using Sirenix.OdinInspector;
    using TMPro;
    using UnityEngine;
    using GameInstance = Core.GameInstance;

    [ExecuteInEditMode]
    public class SubtitleComponent : MonoBehaviour
    {
        [TitleGroup(Constants.InspectorGroups.References)] 
        [SerializeField] private TextMeshProUGUI m_subtitleSpeakerName = null;
        [SerializeField] private TextMeshProUGUI m_subtitleText = null;
        [SerializeField] private CanvasGroup m_subtitleCanvasGroup = null;
        [SerializeField] private CanvasGroup m_subtitleBackGround = null;

        [TitleGroup(Constants.InspectorGroups.Settings)] 
        [SerializeField] private bool m_isMainSubtitleComponent = true;

        private bool m_showSubtitles = false;
        private bool m_showBackground = false;

        private UserSettingsData m_userSettingsData = null;
        private UserSettingsData UserSettingsData
        {
            get
            {
                if (m_userSettingsData == null)
                {
                    m_userSettingsData = UserSettingsManager.Instance.GetUserSettingsData();
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
            ShowSubtitleSettingEvent.Register(UpdateShowSubtitleFromSettings);
            ShowSpeakerNameSettingEvents.Register(UpdateShowSpeakerNameFromSettings);
            ChangeSubtitleBackgroundOpacityEvent.Register(UpdateBackgroundOpacityFromSettings);
            ShowSubtitleBackgroundSettingEvent.Register(UpdateShowBackgroundFromSettings);

            if (GameInstance.GameInstance.Instance.IsReady)
            {
                UpdateShowSubtitleFromSettings(UserSettingsData.ShowSubtitles);
                UpdateShowSpeakerNameFromSettings(UserSettingsData.ShowSubtitleSpeakerName);
            }
        }

        private void OnDisable()
        {
            ShowSubtitleSettingEvent.Unregister(UpdateShowSubtitleFromSettings);
            ShowSpeakerNameSettingEvents.Unregister(UpdateShowSpeakerNameFromSettings);
            ChangeSubtitleBackgroundOpacityEvent.Unregister(UpdateBackgroundOpacityFromSettings);
            ShowSubtitleBackgroundSettingEvent.Unregister(UpdateShowBackgroundFromSettings);
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

        private void UpdateShowSpeakerNameFromSettings(bool showSubtitleSpeakerName)
        {
            m_subtitleSpeakerName.gameObject.SetActive(showSubtitleSpeakerName);
        }

        private void UpdateBackgroundOpacityFromSettings(int subtitleBackgroundOpacity)
        {
            if (m_showBackground == false)
            {
                m_subtitleBackGround.alpha = 0f;
                return;
            }

            m_subtitleBackGround.alpha = (float)subtitleBackgroundOpacity / 100;
        }

        private void UpdateShowBackgroundFromSettings(bool showSubtitleBackground)
        {
            m_showBackground = showSubtitleBackground;
        }
    }
}
