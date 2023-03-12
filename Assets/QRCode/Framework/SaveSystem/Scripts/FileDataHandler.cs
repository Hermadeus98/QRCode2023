namespace QRCode.Framework
{
    using System.IO;
    using System.Threading.Tasks;
    using Formatters;

    /// <summary>
    /// Default File Data Handler, used for PC save and Editor Save.
    /// </summary>
    public class FileDataHandler : IFileDataHandler
    {
        private string m_dataDirectoryPath = "";
        private string m_dataFileName = "";

        private string m_fullPath = "";
        private IFormatter m_formatter = null;
        
        public FileDataHandler(string dataDirectoryPath, string dataFileName)
        {
            m_dataDirectoryPath = dataDirectoryPath;
            m_dataFileName = dataFileName;
            
            m_fullPath = Path.Combine(m_dataDirectoryPath, m_dataFileName);
            m_formatter = FormatterFactory.CreateFormatter(SaveServiceSettings.Instance.FormatterTypeDefault);
        }

        public async Task<GameData> Load()
        {
            var loadedObject = await m_formatter.Load<GameData>(m_fullPath);

            return loadedObject;
        }
        
        public async Task Save(GameData gameData)
        {
            await m_formatter.Save(gameData, m_fullPath);
        }

        public async Task<bool> TryDeleteSave()
        {
            var task = m_formatter.TryDeleteFile(m_fullPath);
            var result = await task;
            
            return result;
        }
    }
}