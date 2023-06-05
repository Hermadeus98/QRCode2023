namespace QRCode.Framework
{
    using System;
    using Settings.InterfaceSettings;

    public class UserSettingsEvents
    {
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

        public UserSettingsEvents()
        {
            
        }
        
        public void UpdateTextSizeSettings(TextSizeSetting textSizeSetting)
        {
            m_onTextSizeSettingChange.Invoke(textSizeSetting);
        }

        public void RaiseEvents()
        {
            var userSettingsData = ServiceLocator.Current.Get<IUserSettingsService>().GetUserSettingsData();
            
            UpdateTextSizeSettings(userSettingsData.TextSizeSetting);
        }
    }
}