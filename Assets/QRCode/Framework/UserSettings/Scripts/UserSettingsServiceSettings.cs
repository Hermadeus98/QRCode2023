namespace QRCode.Framework
{
    using Sirenix.OdinInspector;
    using UnityEngine;

    [CreateAssetMenu(menuName = K.UserSettings.UserSettingsSettingsPath, fileName = "STG_User Settings Service Settings")]
    public class UserSettingsServiceSettings : Settings<UserSettingsServiceSettings>
    {
        [TitleGroup("General Settings")] 
        [SerializeField] [SuffixLabel("@this.m_fileNameExtension")] 
        private string m_fileName = "userSettings";
        [SerializeField] 
        private string m_fileNameExtension = ".save";

        public string FullFileName => m_fileName + m_fileNameExtension;
    }
}
