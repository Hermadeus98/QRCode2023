namespace QRCode.Engine.Core.UserSettings
{
    using System.Threading;
    using System.Threading.Tasks;
    using QRCode.Engine.Core.Manager;
    using QRCode.Engine.Core.SaveSystem;
    using QRCode.Engine.Core.Tags;
    using QRCode.Engine.Debugging;
    using QRCode.Engine.Toolbox.Optimization;

    /// <summary>
    /// This class manage all the user settings.
    /// </summary>
    public class UserSettingsManager : GenericManagerBase<UserSettingsManager>, IDeletable
    {
        #region Fields
        private UserSettingsData _userSettingsData = null;
        private IFileDataHandler _fileDataHandler = null;
        #endregion Fields

        #region Properties
        /// <summary>
        /// Return the current <see cref="_userSettingsData"/>.
        /// </summary>
        public UserSettingsData GetUserSettingsData { get { return _userSettingsData; } }
        #endregion Properties
        
        #region Methods
        #region LifeCycle
        protected override async Task InitAsync(CancellationToken cancellationToken)
        {
            var saveServiceSettings = SaveServiceSettings.Instance;
            var userSettingsSettings = UserSettingsServiceSettings.Instance;
            _fileDataHandler = FileDataHandlerFactory.CreateFileDataHandler(saveServiceSettings.FullPath, userSettingsSettings.FullFileName);
            await LoadUserSettingsData();
            
            UserSettingsEvents.RaiseUserSettingsEvents();
        }

        public override void Delete()
        {
            _userSettingsData = null;
            
            if (_fileDataHandler != null)
            {
                _fileDataHandler.Dispose();
                _fileDataHandler = null;
            }
            
            base.Delete();
        }
        #endregion LifeCycle

        #region Public Methods
        /// <summary>
        /// Create new <see cref="UserSettingsData"/> with default values.
        /// </summary>
        private async Task CreateNewUserSettingsData()
        {
            _userSettingsData = new UserSettingsData();
            await SaveUserSettingsData();
        }
        
        /// <summary>
        /// Save <see cref="UserSettingsData"/>.
        /// </summary>
        public async Task SaveUserSettingsData()
        {
            await _fileDataHandler.Save(_userSettingsData);
            
            QRLogger.Debug<CoreTags.UserSettings>($"User Settings is save.");
        }

        /// <summary>
        /// Will raise <see cref="UserSettingsEvents"/>.
        /// </summary>
        public void ApplyChange()
        {
            UserSettingsEvents.RaiseUserSettingsEvents();
        }
        #endregion Public Methods

        #region Private Methods
        private async Task LoadUserSettingsData()
        {
            _userSettingsData = await _fileDataHandler.Load<UserSettingsData>();
            
            if (_userSettingsData == null)
            {
                QRLogger.Debug<CoreTags.UserSettings>($"No {nameof(_userSettingsData)} was found. Initializing default values.");
                await CreateNewUserSettingsData();
            }
            
            QRLogger.Debug<CoreTags.UserSettings>($"User Settings are load.");
        }
        #endregion Private Methods
        #endregion Methods
    }
}