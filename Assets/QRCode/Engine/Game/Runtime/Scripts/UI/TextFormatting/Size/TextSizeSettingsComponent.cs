namespace QRCode.Engine.Game.UI.Components
{
    using System.Threading.Tasks;
    using Toolbox;
    using Engine.Core.GameInstance;
    using Engine.Core.UserSettings;
    using Engine.Core.UserSettings.Events.InterfaceSettings;
    using GameConfigs;
    using Sirenix.OdinInspector;
    using TMPro;
    using Toolbox.GameConfigs;
    using UnityEngine;

    public class TextSizeSettingsComponent : MonoBehaviour
    {
        [TitleGroup(Constants.InspectorGroups.References)]
        [SerializeField] private TextMeshProUGUI m_textMeshProUGUI = null;

        [TitleGroup(Constants.InspectorGroups.Settings)] [SerializeField]
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
                    m_userSettingsData = UserSettingsManager.Instance.GetUserSettingsData;
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
            
            UpdateTextFromSettings(UserSettingsData.TextSizeSetting);
        }

        private void OnDisable()
        {
            TextSizeSettingEvent.Unregister(UpdateTextFromSettings);
        }

        [Button]
        private void UpdateTextFromSettings(Engine.Core.UserSettings.Settings.InterfaceSettings.TextSizeSetting textSizeSetting)
        {
            var textRuleSetCatalog = GameConfigs.GetCatalogOfType<TextGameConfig>();
            var textRuleSet = textRuleSetCatalog.GetDataFromId(m_textRuleSetName);
            var setting = textRuleSet.GetTextSetting(textSizeSetting);
            m_textMeshProUGUI.fontSize = setting.FontSize;
        }
    }
}
