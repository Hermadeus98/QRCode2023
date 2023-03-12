namespace QRCode.Framework
{
    using Singleton;
    using Sirenix.OdinInspector;
    using UnityEngine;

    [CreateAssetMenu(fileName = "STG_SaveService", menuName = K.SettingsPath.SaveSystemSettingsPath, order = 0)]
    public class SaveServiceSettings : SingletonScriptableObject<SaveServiceSettings>
    {
        [TitleGroup("General Settings")] 
        [SerializeField] private string m_fileName = "save";
        [SerializeField] private string m_fileNameExtension = ".save";
        [SerializeField] private bool m_useEncryption = true;
        
        [TitleGroup("Default")] 
        [SerializeField] private bool m_useApplicationPersistentDataPath = true;
        [TitleGroup("Default")] 
        [SerializeField][ShowIf("@this.m_useApplicationPersistentDataPath == false")] private string m_dataDirectoryPathDefault = "";

        [TitleGroup("Default")] [SerializeField]
        private FormatterTypeEnum m_formatterType = FormatterTypeEnum.JSON;

        public string FullPath
        {
            get
            {
                return m_useApplicationPersistentDataPath ? Application.persistentDataPath : m_dataDirectoryPathDefault;
            }
        }

        public string FileNameExtension => m_fileNameExtension;
        public string FileName => m_fileName;
        public string FullFileName => m_fileName + m_fileNameExtension;

        public FormatterTypeEnum FormatterTypeDefault => m_formatterType;
        public bool UseEncryption => m_useEncryption;
    }
}