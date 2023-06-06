namespace QRCode.Framework
{
    using System.Threading;
    using System.Threading.Tasks;
    using Game;
    using Settings.InterfaceSettings;
    using Sirenix.OdinInspector;
    using TMPro;
    using UnityEngine;

    public class SubtitleTextSizeSettingsComponent : MonoBehaviour
    {
        [TitleGroup(K.InspectorGroups.References)]
        [SerializeField] private TextMeshProUGUI m_textMeshProUGUI = null;

        [TitleGroup(K.InspectorGroups.Settings)] [SerializeField]
        private string m_textRuleSetName = "Default";

        private CancellationTokenSource m_cancellationTokenSource = null;
        private IUserSettingsService m_userSettingsService = null;

        private IUserSettingsService UserSettingsService
        {
            get
            {
                if (m_userSettingsService == null)
                {
                    m_userSettingsService = ServiceLocator.Current.Get<IUserSettingsService>();
                }

                return m_userSettingsService;
            }
        }
        
        private Catalog m_catalog = null;
        private Catalog Catalog
        {
            get
            {
                if (m_catalog == null)
                {
                    m_catalog = Catalog.Instance;
                }

                return m_catalog;
            }
        }

        private async void OnEnable()
        {
            m_cancellationTokenSource = new CancellationTokenSource();
            while (Bootstrap.IsInit() == false && m_cancellationTokenSource.Token.IsCancellationRequested == false)
            {
                await Task.Yield();
            }
            m_cancellationTokenSource.Dispose();
            
            UserSettingsService.UserSettingsEvents.OnSubtitleTextSizeSettingChange += UpdateTextFromSettings;
            UpdateTextFromSettings(m_userSettingsService.GetUserSettingsData().SubtitlesTextSizeSetting);
        }

        private void OnDisable()
        {
            UserSettingsService.UserSettingsEvents.OnSubtitleTextSizeSettingChange -= UpdateTextFromSettings;
        }

        [Button]
        private void UpdateTextFromSettings(TextSizeSetting subtitlesTextSizeSetting)
        {
            var textRuleSetCatalog = Catalog.GetCatalogOfType<TextRuleSetCatalog>();
            var textRuleSet = textRuleSetCatalog.GetDataFromId(m_textRuleSetName);
            var setting = textRuleSet.GetTextSetting(subtitlesTextSizeSetting);
            m_textMeshProUGUI.fontSize = setting.FontSize;
        }

        private void OnValidate()
        {
            if (m_textMeshProUGUI == null)
            {
                m_textMeshProUGUI = GetComponent<TextMeshProUGUI>();
                UpdateTextFromSettings(m_userSettingsService.GetUserSettingsData().TextSizeSetting);
            }
        }
    }
}