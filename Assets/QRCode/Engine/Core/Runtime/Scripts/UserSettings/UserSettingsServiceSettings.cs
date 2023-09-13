namespace QRCode.Engine.Core.UserSettings
{
    using System.Threading.Tasks;
    using QRCode.Engine.Constants;
    using QRCode.Engine.Core.SaveSystem;
    using QRCode.Engine.Core.Tags;
    using QRCode.Engine.Debugging;
    using QRCode.Engine.Toolbox.Settings;
    using Sirenix.OdinInspector;
    using UnityEngine;

    [CreateAssetMenu(menuName = UserSettingsConstants.UserSettings.UserSettingsSettingsPath, fileName = "STG_UserSettingsSettings")]
    public class UserSettingsServiceSettings : Settings<UserSettingsServiceSettings>
    {
        #region Fields
        [TitleGroup("General Settings")] 
        [Tooltip("The file name where all the user settings will be saved.")]
        [SuffixLabel("@this._fileNameExtension")] 
        [SerializeField] private string _fileName = "userSettings";
        
        [TitleGroup("General Settings")] 
        [Tooltip("The extension of the file where all the user settings will be saved.")]
        [SerializeField] private string _fileNameExtension = ".save";

        [TitleGroup("Default Values")]
        [Tooltip("The default values used to create a new User Settings at the first initialization of the application.")]
        [SerializeField] private UserSettingsData _defaultValues = new UserSettingsData();
        #endregion Fields

        #region Properties
        /// <summary>
        /// FullFileName correspond to the fileName + fileNameExtension.
        /// </summary>
        public string FullFileName => _fileName + _fileNameExtension;
        #endregion Properties

        [Button]
        private void ResetDefaultValues()
        {
            _defaultValues = new UserSettingsData();
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
            await fileDataHandler.Save(_defaultValues);
            QRLogger.Debug<CoreTags.UserSettings>($"User Settings has been changed.");
        }

        private void OnValidate()
        {
            if (Application.isPlaying)
            {
                ApplyChange();
            }
        }

        private void ApplyChange()
        {
            var userSettingService = UserSettingsManager.Instance;
            var userSettings = userSettingService.GetUserSettingsData;

            //CONTROLS
            userSettings.MenuHoldFactor = _defaultValues.MenuHoldFactor;
            
            //INTERFACE
            userSettings.InterfaceAreaCalibrationSize = _defaultValues.InterfaceAreaCalibrationSize;
            userSettings.TextSizeSetting = _defaultValues.TextSizeSetting;
            userSettings.GamepadCursorSensibility = _defaultValues.GamepadCursorSensibility;
            userSettings.MenuNavigationSettings = _defaultValues.MenuNavigationSettings;

            //SOUND
            userSettings.VoiceLanguage = _defaultValues.VoiceLanguage;
            userSettings.ShowSubtitles = _defaultValues.ShowSubtitles;
            userSettings.ShowSubtitleBackground = _defaultValues.ShowSubtitleBackground;
            userSettings.SubtitleBackgroundOpacity = _defaultValues.SubtitleBackgroundOpacity;
            userSettings.ShowSubtitleSpeakerName = _defaultValues.ShowSubtitleSpeakerName;
            userSettings.SubtitlesTextSizeSetting = _defaultValues.SubtitlesTextSizeSetting;
            
            userSettingService.ApplyChange();
        }
    }
}
