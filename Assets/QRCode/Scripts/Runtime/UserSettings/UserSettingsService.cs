namespace QRCode.Framework
{
    using System.Threading.Tasks;
    using Debugging;

    public class UserSettingsService : IUserSettingsService
    {
        private UserSettingsData m_userSettingsData = null;
        private IFileDataHandler m_fileDataHandler = null;

        private bool m_IsInit = false;
        
        public async Task Initialize()
        {
            if (m_IsInit)
            {
                return;
            }
            
            var saveServiceSettings = SaveServiceSettings.Instance;
            var userSettingsSettings = UserSettingsServiceSettings.Instance;
            m_fileDataHandler = FileDataHandlerFactory.CreateFileDataHandler(saveServiceSettings.FullPath, userSettingsSettings.FullFileName);
            await LoadUserSettingsData();
            
#if UNITY_EDITOR && USE_NOT_BOOTSTRAP_DEBUG
            //This delay should not exist in build, the bootstrap must be finish before launch first game scene.
            await Task.Delay(500);
#endif
            UserSettingsEvents.RaiseUserSettingsEvents();
        }

        public UserSettingsData GetUserSettingsData()
        {
            return m_userSettingsData;
        }

        public async Task CreateUserSettingsData()
        {
            m_userSettingsData = new UserSettingsData();
            await SaveUserSettingsData();
        }

        public async Task LoadUserSettingsData()
        {
            m_userSettingsData = await m_fileDataHandler.Load<UserSettingsData>();
            
            if (m_userSettingsData == null)
            {
                QRDebug.Debug(K.DebuggingChannels.UserSettings, $"No {nameof(m_userSettingsData)} was found. Initializing default values.");
                await CreateUserSettingsData();
            }
            
            QRDebug.Debug(K.DebuggingChannels.UserSettings,$"User Settings are load.");
        }

        public async Task SaveUserSettingsData()
        {
            await m_fileDataHandler.Save(m_userSettingsData);
            
            QRDebug.Debug(K.DebuggingChannels.UserSettings,$"User Settings is save.");
        }

        public void ApplyChange(UserSettingsData newUserSettingsData = null)
        {
            UserSettingsEvents.RaiseUserSettingsEvents();
        }
    }
}