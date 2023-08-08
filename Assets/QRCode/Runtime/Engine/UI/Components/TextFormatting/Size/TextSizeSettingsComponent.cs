namespace QRCode.Framework
{
    using Events;
    using Game;
    using Settings.InterfaceSettings;
    using Sirenix.OdinInspector;
    using TMPro;
    using UnityEngine;

    public class TextSizeSettingsComponent : MonoBehaviour
    {
        [TitleGroup(K.InspectorGroups.References)]
        [SerializeField] private TextMeshProUGUI m_textMeshProUGUI = null;

        [TitleGroup(K.InspectorGroups.Settings)] [SerializeField]
        private string m_textRuleSetName = "Default";

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

        private void OnEnable()
        {
            TextSizeSettingEvent.Register(UpdateTextFromSettings);

            if (BootstrapOld.IsInit())
            {
                UpdateTextFromSettings(UserSettingsData.TextSizeSetting);
            }
        }

        private void OnDisable()
        {
            TextSizeSettingEvent.Unregister(UpdateTextFromSettings);
        }

        [Button]
        private void UpdateTextFromSettings(Settings.InterfaceSettings.TextSizeSetting textSizeSetting)
        {
            var textRuleSetCatalog = Catalog.GetCatalogOfType<TextRuleSetCatalog>();
            var textRuleSet = textRuleSetCatalog.GetDataFromId(m_textRuleSetName);
            var setting = textRuleSet.GetTextSetting(textSizeSetting);
            m_textMeshProUGUI.fontSize = setting.FontSize;
        }
    }
}
