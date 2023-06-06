namespace QRCode.Framework
{
    using System.Threading.Tasks;
    using Debugging;
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

        [TitleGroup("Default Values")]
        [SerializeField] 
        private UserSettingsData m_defaultValues = new UserSettingsData();
        
        public string FullFileName => m_fileName + m_fileNameExtension;
        public UserSettingsData DefaultUserSettingsData => m_defaultValues;

        [Button]
        private void ResetDefaultValues()
        {
            m_defaultValues = new UserSettingsData();
        }
        
        [Button]
        private async void SaveAsDefault()
        {
            await SaveAsDefaultTask();
        }

        private async Task SaveAsDefaultTask()
        {
            var saveServiceSettings = SaveServiceSettings.Instance;
            var fileDataHandler = FileDataHandlerFactory.CreateFileDataHandler(saveServiceSettings.FullPath, FullFileName);
            await fileDataHandler.Save(m_defaultValues);
            QRDebug.Debug(K.DebuggingChannels.Editor,$"User Settings has been changed.");
        }

        [Button()]
        private async void ApplyChange()
        {
            var userSettingService = ServiceLocator.Current.Get<IUserSettingsService>();

            await SaveAsDefaultTask();
            await userSettingService.LoadUserSettingsData();
            
            userSettingService.UserSettingsEvents.RaiseEvents();
        }
    }
}
