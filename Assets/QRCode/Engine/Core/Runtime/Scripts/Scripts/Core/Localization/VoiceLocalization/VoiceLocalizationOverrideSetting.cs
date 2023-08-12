namespace QRCode.Engine.Core.Localization
{
    using UnityEngine.Localization.Settings;
    using System.Threading.Tasks;
    using Toolbox;
    using Engine.Core.GameInstance;
    using Engine.Core.UserSettings;
    using Engine.Core.UserSettings.Events.SoundSettings;
    using Framework;
    using Sirenix.OdinInspector;
    using Toolbox.Database;
    using Toolbox.Database.GeneratedEnums;
    using UnityEngine;
    using UnityEngine.Localization;
    using UnityEngine.Localization.Components;

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
                    m_userSettingsData = UserSettingsManager.Instance.GetUserSettingsData();
                }

                return m_userSettingsData;
            }
        }

        private AvailableVoiceLocalizationDatabase m_availableVoiceLocalizationDatabase = null;

        private AvailableVoiceLocalizationDatabase AvailableVoiceLocalizationDatabase
        {
            get
            {
                if (m_availableVoiceLocalizationDatabase == null)
                {
                    DB.Instance.TryGetDatabase(DBEnum.DB_AvailableVoiceLocales, out m_availableVoiceLocalizationDatabase);
                }

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
            
            if (UserSettingsManager.Instance.IsInit && !operation.IsDone)
            {
                AvailableVoiceLocalizationDatabase.TryGetInDatabase(UserSettingsData.VoiceLanguage.ToString(), out var foundedLocale);
                UpdateAudioLocaleFromSettings(foundedLocale);
            }
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
