namespace QRCode.Framework
{
    using System;
    using Settings.InterfaceSettings;

    public class UserSettingsEvents
    {
        #region TEXT SIZE SETTING
        private Action<TextSizeSetting> m_onTextSizeSettingChange;
        public event Action<TextSizeSetting> OnTextSizeSettingChange
        {
            add
            {
                m_onTextSizeSettingChange -= value;
                m_onTextSizeSettingChange += value;
            }
            remove
            {
                m_onTextSizeSettingChange -= value;
            }
        }
        
        public void UpdateTextSizeSettings(TextSizeSetting textSizeSetting)
        {
            m_onTextSizeSettingChange.Invoke(textSizeSetting);
        }
        #endregion TEXT SIZE SETTING

        #region SUBTITLE TEXT SIZE SETTING
        private Action<TextSizeSetting> m_onSubtitleTextSizeSettingChange;
        public event Action<TextSizeSetting> OnSubtitleTextSizeSettingChange
        {
            add
            {
                m_onSubtitleTextSizeSettingChange -= value;
                m_onSubtitleTextSizeSettingChange += value;
            }
            remove
            {
                m_onSubtitleTextSizeSettingChange -= value;
            }
        }
        
        public void UpdateSubtitleTextSizeSettings(TextSizeSetting subtitlesTextSizeSetting)
        {
            m_onSubtitleTextSizeSettingChange.Invoke(subtitlesTextSizeSetting);
        }
        #endregion SUBTITLE TEXT SIZE SETTING

        public UserSettingsEvents()
        {
            
        }
        
        public void RaiseEvents()
        {
            var userSettingsData = ServiceLocator.Current.Get<IUserSettingsService>().GetUserSettingsData();
            
            UpdateTextSizeSettings(userSettingsData.TextSizeSetting);
            UpdateSubtitleTextSizeSettings(userSettingsData.SubtitlesTextSizeSetting);
        }
    }
}