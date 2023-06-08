namespace QRCode.Framework
{
    using Events;
    using UnityEngine;

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
            //INTERFACE
            TextSizeSettingEvent.Trigger(UserSettingsService.TextSizeSetting);
            
            //SOUND
            SubtitlesTextSizeSettingEvent.Trigger(UserSettingsService.SubtitlesTextSizeSetting);
            ShowSubtitleBackgroundSettingEvent.Trigger(UserSettingsService.ShowSubtitleBackground);
            ChangeSubtitleBackgroundOpacityEvent.Trigger(UserSettingsService.SubtitleBackgroundOpacity);
            ShowSpeakerNameSettingEvents.Trigger(UserSettingsService.ShowSubtitleSpeakerName);
            ShowSubtitleSettingEvent.Trigger(UserSettingsService.ShowSubtitles);
        }
    }
}