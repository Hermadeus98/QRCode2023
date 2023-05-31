namespace QRCode.Framework
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Debugging;
    using Formatters;

    /// <summary>
    /// Default File Data Handler, used for PC save and Editor Save.
    /// </summary>
    public class FileDataHandler : IFileDataHandler
    {
        private readonly string m_fullPath;
        private readonly IFormatter m_formatter = null;
        
        public FileDataHandler(string dataDirectoryPath, string dataFileName)
        {
            m_fullPath = Path.Combine(dataDirectoryPath, dataFileName);
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

        public Task<bool> TryDeleteSave()
        {
            try
            {
                if (File.Exists(m_fullPath))
                {
                    File.Delete(m_fullPath);
                    return Task.FromResult(true);
                }
                else
                {
                    QRDebug.DebugFatal(K.DebuggingChannels.SaveSystem, $"Cannot delete any file at path {m_fullPath}.");
                }
            }
            catch (Exception e)
            {
                QRDebug.DebugFatal(K.DebuggingChannels.SaveSystem, e);
                throw;
            }

            return Task.FromResult(false);
        }
    }
}