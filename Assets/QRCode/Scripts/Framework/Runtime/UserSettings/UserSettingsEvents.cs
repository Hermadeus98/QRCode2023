namespace QRCode.Framework
{
    using Events;
    using UnityEngine;
    using UnityEngine.InputSystem;

    public struct UserSettingsEvents
    {
        private static UserSettingsData m_userSettingsService = null;

        private static UserSettingsData UserSettingsService
        {
            get
            {
                if (m_userSettingsService == null)
                {
                    m_userSettingsService = ServiceLocator.Current.Get<IUserSettingsService>().GetUserSettingsData();
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
            DB.Instance.TryGetDatabase<AvailableVoiceLocalizationDatabase>(DBEnum.DB_AvailableVoiceLocales, out var availableVoiceLocalizationDatabase);
            availableVoiceLocalizationDatabase.TryGetInDatabase(UserSettingsService.VoiceLanguage.ToString(), out var foundedLocale);
            
            //CONTROLS
            InputSystem.settings.defaultHoldTime = (float)UserSettingsService.MenuHoldFactor / 1000;
            
            //INTERFACE
            InterfaceAreaCalibrationEvent.Trigger(UserSettingsService.InterfaceAreaCalibrationSize);
            TextSizeSettingEvent.Trigger(UserSettingsService.TextSizeSetting);
            GamepadCursorSensibilityEvent.Trigger(UserSettingsService.GamepadCursorSensibility);
            
            //SOUND
            VoiceLanguageSettingEvent.Trigger(foundedLocale);
            SubtitlesTextSizeSettingEvent.Trigger(UserSettingsService.SubtitlesTextSizeSetting);
            ShowSubtitleBackgroundSettingEvent.Trigger(UserSettingsService.ShowSubtitleBackground);
            ChangeSubtitleBackgroundOpacityEvent.Trigger(UserSettingsService.SubtitleBackgroundOpacity);
            ShowSpeakerNameSettingEvents.Trigger(UserSettingsService.ShowSubtitleSpeakerName);
            ShowSubtitleSettingEvent.Trigger(UserSettingsService.ShowSubtitles);
        }
    }
}