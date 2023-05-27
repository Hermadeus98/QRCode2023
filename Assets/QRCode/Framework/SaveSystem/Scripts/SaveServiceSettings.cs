namespace QRCode.Framework
{
    using Sirenix.OdinInspector;
    using UnityEngine;

    [CreateAssetMenu(fileName = "STG_SaveService", menuName = K.SettingsPath.SaveSystemSettingsPath, order = 0)]
    public class SaveServiceSettings : Settings<SaveServiceSettings>
    {
        [TitleGroup("General Settings")] 
        [Tooltip("The name of the file containing the save data.")]
        [SerializeField] [SuffixLabel("@this.m_fileNameExtension")] private string m_fileName = "save";
        [Tooltip("The extension of the file containing the save data. Ex: file.save")]
        [SerializeField] private string m_fileNameExtension = ".save";
        [Tooltip("Encryption allow to get unreadable save file.")]
        [SerializeField] private bool m_useEncryption = true;
        [Tooltip("Add a save step before loading new scene.")]
        [SerializeField] private bool m_saveAsyncBeforeSceneLoading = true;
        [Tooltip("Automatic load after scene loading.")]
        [SerializeField] private bool m_loadAsyncAfterSceneLoading = true;
 
        [TitleGroup("Default")] 
        [Tooltip("RECOMMENDED : Keep this setting as TRUE to save on the application persistent data path.")]
        [SerializeField] private bool m_useApplicationPersistentDataPath = true;
        [TitleGroup("Default")] 
        [SerializeField][ShowIf("@this.m_useApplicationPersistentDataPath == false")] 
        private string m_dataDirectoryPathDefault = "";

        [TitleGroup("Default")] [SerializeField]
        private FormatterTypeEnum m_formatterType = FormatterTypeEnum.JSON;
        
        [TitleGroup(K.InspectorGroups.Debugging)]
        [Tooltip("Fake Save with save but will take add fake duration to the save to imitate a real save at more advanced step of production.")]
        [SerializeField] private bool m_useFakeSave = true;
        [Tooltip("Duration of the fake save.")]
        [SerializeField][SuffixLabel("s")] private float m_fakeSaveDuration = 2f;

        [ShowInInspector] [ReadOnly] public string FullPath => m_useApplicationPersistentDataPath ? Application.persistentDataPath : m_dataDirectoryPathDefault;
        public string FullFileName => m_fileName + m_fileNameExtension;
        public FormatterTypeEnum FormatterTypeDefault => m_formatterType;
        public bool UseEncryption => m_useEncryption;
        public bool SaveAsyncBeforeSceneLoading => m_saveAsyncBeforeSceneLoading;
        public bool LoadAsyncAfterSceneLoading => m_loadAsyncAfterSceneLoading;
        public bool UseFakeSave => m_useFakeSave;
        public float FakeSaveDuration => m_fakeSaveDuration;
    }
}