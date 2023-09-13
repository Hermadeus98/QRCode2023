namespace QRCode.Engine.Core.UserSettings
{
    using Events.InterfaceSettings;
    using Events.SoundSettings;
    using Localization;
    using Toolbox.Database;
    using Toolbox.Database.GeneratedEnums;
    using UnityEngine;
    using UnityEngine.InputSystem;

    public struct UserSettingsEvents
    {
        private static UserSettingsManager m_userSettingsService = null;

        private static UserSettingsManager UserSettingsService
        {
            get
            {
                if (m_userSettingsService == null)
                {
                    m_userSettingsService = UserSettingsManager.Instance;
                }

                return m_userSettingsService;
            }
        }


        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void RuntimeInitialization()
        {
            m_userSettingsService = null;
        }
        
        public static void RaiseUserSettingsEvents()
        {
            var userSettingsData = UserSettingsService.GetUserSettingsData;
            
            var availableVoiceLocalizationDatabase = DB.Instance.GetDatabase<AvailableVoiceLocalizationDatabase>(DBEnum.DB_AvailableVoiceLocales);
            availableVoiceLocalizationDatabase.TryGetInDatabase(userSettingsData.VoiceLanguage.ToString(), out var foundedLocale);
            
            //CONTROLS
            InputSystem.settings.defaultHoldTime = (float)userSettingsData.MenuHoldFactor / 1000;
            
            //INTERFACE
            InterfaceAreaCalibrationEvent.Trigger(userSettingsData.InterfaceAreaCalibrationSize);
            TextSizeSettingEvent.Trigger(userSettingsData.TextSizeSetting);
            GamepadCursorSensibilityEvent.Trigger(userSettingsData.GamepadCursorSensibility);
            MenuNavigationSettingEvent.Trigger(userSettingsData.MenuNavigationSettings);
            
            //SOUND
            VoiceLanguageSettingEvent.Trigger(foundedLocale);
            SubtitlesTextSizeSettingEvent.Trigger(userSettingsData.SubtitlesTextSizeSetting);
            ShowSubtitleBackgroundSettingEvent.Trigger(userSettingsData.ShowSubtitleBackground);
            ChangeSubtitleBackgroundOpacityEvent.Trigger(userSettingsData.SubtitleBackgroundOpacity);
            ShowSpeakerNameSettingEvents.Trigger(userSettingsData.ShowSubtitleSpeakerName);
            ShowSubtitleSettingEvent.Trigger(userSettingsData.ShowSubtitles);
        }
    }
}