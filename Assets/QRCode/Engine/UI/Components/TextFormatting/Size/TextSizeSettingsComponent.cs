namespace QRCode.Framework
{
    using System.Threading.Tasks;
    using Engine.Core;
    using Events;
    using Sirenix.OdinInspector;
    using TMPro;
    using UnityEngine;

    public class TextSizeSettingsComponent : MonoBehaviour
    {
        [TitleGroup(K.InspectorGroups.References)]
        [SerializeField] private TextMeshProUGUI m_textMeshProUGUI = null;

        [TitleGroup(K.InspectorGroups.Settings)] [SerializeField]
        private string m_textRuleSetName = "Default";

        private GameConfigs m_gameConfigs = null;
        private GameConfigs GameConfigs
        {
            get
            {
                if (m_gameConfigs == null)
                {
                    m_gameConfigs = GameConfigs.Instance;
                }

                return m_gameConfigs;
            }
        }
        
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

        private async void OnEnable()
        {
            TextSizeSettingEvent.Register(UpdateTextFromSettings);

            while (GameInstance.Instance.IsReady == false)
            {
                await Task.Yield();
            }
            
            if (UserSettingsManager.Instance.IsInit)
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
            var textRuleSetCatalog = GameConfigs.GetCatalogOfType<TextGameConfig>();
            var textRuleSet = textRuleSetCatalog.GetDataFromId(m_textRuleSetName);
            var setting = textRuleSet.GetTextSetting(textSizeSetting);
            m_textMeshProUGUI.fontSize = setting.FontSize;
        }
    }
}
