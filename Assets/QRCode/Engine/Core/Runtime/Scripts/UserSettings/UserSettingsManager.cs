namespace QRCode.Engine.Core.UserSettings
{
    using System.Threading;
    using System.Threading.Tasks;
    
    using Managers;
    using SaveSystem;
    using Debugging;
    using Toolbox;
    using Toolbox.Pattern.Singleton;

    public class UserSettingsManager : MonoBehaviourSingleton<UserSettingsManager>, IUserSettingsService, IManager
    {
        private UserSettingsData m_userSettingsData = null;
        private IFileDataHandler m_fileDataHandler = null;

        private bool m_IsInit = false;

        public bool IsInit
        {
            get
            {
                return m_IsInit;
            }
        }
        
        public async Task InitAsync(CancellationToken cancellationToken)
        {
            if (m_IsInit)
            {
                m_IsInit = true;
                return;
            }
            
            var saveServiceSettings = SaveServiceSettings.Instance;
            var userSettingsSettings = UserSettingsServiceSettings.Instance;
            m_fileDataHandler = FileDataHandlerFactory.CreateFileDataHandler(saveServiceSettings.FullPath, userSettingsSettings.FullFileName);
            await LoadUserSettingsData();
            
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
                QRDebug.Debug(Constants.DebuggingChannels.UserSettings, $"No {nameof(m_userSettingsData)} was found. Initializing default values.");
                await CreateUserSettingsData();
            }
            
            QRDebug.Debug(Constants.DebuggingChannels.UserSettings,$"User Settings are load.");
        }

        public async Task SaveUserSettingsData()
        {
            await m_fileDataHandler.Save(m_userSettingsData);
            
            QRDebug.Debug(Constants.DebuggingChannels.UserSettings,$"User Settings is save.");
        }

        public void ApplyChange(UserSettingsData newUserSettingsData = null)
        {
            UserSettingsEvents.RaiseUserSettingsEvents();
        }
    }
}