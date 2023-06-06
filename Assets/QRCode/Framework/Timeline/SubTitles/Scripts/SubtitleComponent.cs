namespace QRCode.Framework
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Game;
    using Sirenix.OdinInspector;
    using TMPro;
    using UnityEngine;

    [ExecuteInEditMode]
    public class SubtitleComponent : SerializedMonoBehaviour
    {
        [TitleGroup(K.InspectorGroups.References)] [SerializeField]
        private TextMeshProUGUI m_subtitleText = null;

        [TitleGroup(K.InspectorGroups.References)] [SerializeField]
        private CanvasGroup m_subtitleCanvasGroup = null;

        [TitleGroup(K.InspectorGroups.Settings)] [SerializeField]
        private bool m_isMainSubtitleComponent = true;

        private IUserSettingsService m_userSettingsService = null;
        private CancellationTokenSource m_cancellationTokenSource = null;

        public TextMeshProUGUI SubtitleText => m_subtitleText;

        private async void Start()
        {
            if (m_isMainSubtitleComponent)
            {
                Subtitles.Instance.SetMainSubtitleComponent(this);
            }

            m_cancellationTokenSource = new CancellationTokenSource();
            while (Bootstrap.IsInit() == false && m_cancellationTokenSource.Token.IsCancellationRequested == false)
            {
                await Task.Yield();
            }
            m_cancellationTokenSource.Dispose();

            m_userSettingsService = ServiceLocator.Current.Get<IUserSettingsService>();
        }

        private void Update()
        {
#if UNITY_EDITOR
            if (Application.isPlaying == false)
            {
                CheckTextCharacterCount();
            }
#endif
        }

        public void SetTextRaw(string text)
        {
            m_subtitleText.SetText(text);
        }

        public void SetTransparency(float alpha)
        {
            if (Application.isPlaying)
            {
                if (m_userSettingsService != null && m_userSettingsService.GetUserSettingsData().ShowSubtitles == false)
                {
                    alpha = 0f;
                }
            }
            
            CheckTextCharacterCount();

            m_subtitleCanvasGroup.alpha = alpha;
        }

        private void CheckTextCharacterCount()
        {
            if (string.IsNullOrEmpty(m_subtitleText.text))
            {
                m_subtitleCanvasGroup.alpha = 0f;
            }
        }
    }
}
