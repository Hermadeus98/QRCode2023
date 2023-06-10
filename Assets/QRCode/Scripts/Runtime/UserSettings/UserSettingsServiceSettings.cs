namespace QRCode.Framework
{
    using System;
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

        private void OnValidate()
        {
            if (Application.isPlaying)
            {
                ApplyChange();
            }
        }

        private void ApplyChange()
        {
            var userSettingService = ServiceLocator.Current.Get<IUserSettingsService>();
            var userSettings = userSettingService.GetUserSettingsData();

            //INTERFACE
            userSettings.InterfaceAreaCalibrationSize = m_defaultValues.InterfaceAreaCalibrationSize;
            userSettings.TextSizeSetting = m_defaultValues.TextSizeSetting;
            
            //SOUND
            userSettings.ShowSubtitles = m_defaultValues.ShowSubtitles;
            userSettings.ShowSubtitleBackground = m_defaultValues.ShowSubtitleBackground;
            userSettings.SubtitleBackgroundOpacity = m_defaultValues.SubtitleBackgroundOpacity;
            userSettings.ShowSubtitleSpeakerName = m_defaultValues.ShowSubtitleSpeakerName;
            userSettings.SubtitlesTextSizeSetting = m_defaultValues.SubtitlesTextSizeSetting;
            
            userSettingService.ApplyChange(m_defaultValues);
        }
    }
}
