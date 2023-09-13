namespace QRCode.Engine.Core.Localization
{
    using UnityEngine.Localization.Settings;
    using UnityEngine;
    using UnityEngine.Localization;
    using UnityEngine.Localization.Components;
    
    using System.Threading.Tasks;
    
    using Sirenix.OdinInspector;
    
    using Toolbox;
    using GameInstance;
    using UserSettings;
    using UserSettings.Events.SoundSettings;
    using Toolbox.Database;
    using Toolbox.Database.GeneratedEnums;
    

    [RequireComponent(typeof(LocalizeAudioClipEvent))]
    public class VoiceLocalizationOverrideSetting : MonoBehaviour
    {
        [TitleGroup(Constants.InspectorGroups.References)] 
        [SerializeField] private LocalizeAudioClipEvent m_localizeAudioClipEvent = null;

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

        private AvailableVoiceLocalizationDatabase m_availableVoiceLocalizationDatabase = null;

        private AvailableVoiceLocalizationDatabase AvailableVoiceLocalizationDatabase
        {
            get
            {
                m_availableVoiceLocalizationDatabase = DB.Instance.GetDatabase<AvailableVoiceLocalizationDatabase>(DBEnum.DB_AvailableVoiceLocales);
                return m_availableVoiceLocalizationDatabase;
            }
        }

        private async void OnEnable()
        {
            VoiceLanguageSettingEvent.Register(UpdateAudioLocaleFromSettings);
            
            var operation = LocalizationSettings.InitializationOperation;
            
            while (GameInstance.Instance.IsReady == false)
            {
                await Task.Yield();
            }
            
            AvailableVoiceLocalizationDatabase.TryGetInDatabase(UserSettingsData.VoiceLanguage.ToString(), out var foundedLocale);
            UpdateAudioLocaleFromSettings(foundedLocale);
        }

        private void OnDisable()
        {
            VoiceLanguageSettingEvent.Unregister(UpdateAudioLocaleFromSettings);
        }

        [Button]
        private void UpdateAudioLocaleFromSettings(LocaleIdentifier audioLocaleIdentifier)
        {
            m_localizeAudioClipEvent.AssetReference.LocaleOverride = LocalizationSettings.AvailableLocales.GetLocale(audioLocaleIdentifier);
        }
    }
}
