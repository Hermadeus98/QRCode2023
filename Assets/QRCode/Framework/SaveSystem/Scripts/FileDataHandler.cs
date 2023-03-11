namespace QRCode.Framework
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Debugging;
    using UnityEngine;

    public class FileDataHandler : IFileDataHandler
    {
        private string m_dataDirectoryPath = "";
        private string m_dataFileName = "";
        
        public FileDataHandler(string dataDirectoryPath, string dataFileName)
        {
            m_dataDirectoryPath = dataDirectoryPath;
            m_dataFileName = dataFileName;
        }

        public async Task<GameData> Load()
        {
            var fullPath = Path.Combine(m_dataDirectoryPath, m_dataFileName);
            GameData loadedGameData = null;
            
            if (File.Exists(fullPath))
            {
                try
                {
                    var dataToLoad = "";
                    using (var fileStream = new FileStream(fullPath, FileMode.Open))
                    {
                        using (var streamReader = new StreamReader(fileStream))
                        {
                            dataToLoad = await streamReader.ReadToEndAsync();
                        }
                    }

                    loadedGameData = JsonUtility.FromJson<GameData>(dataToLoad);
                }
                catch (Exception e)
                {
                    QRDebug.DebugFatal(K.DebuggingChannels.SaveSystem, e);
                    throw;
                }    
            }

            return loadedGameData;
        }
        
        public async Task Save(GameData gameData)
        {
            var fullPath = Path.Combine(m_dataDirectoryPath, SaveServiceSettings.Instance.FullFileName);
            var dataToStore = JsonUtility.ToJson(gameData, true);
            
            try
            {
                var directoryName = Path.GetDirectoryName(fullPath);
                if (directoryName != null)
                {
                    Directory.CreateDirectory(directoryName);

                    using (var fileStream = new FileStream(fullPath, FileMode.Create))
                    {
                        using (var streamWriter = new StreamWriter(fileStream))
                        {
                            await streamWriter.WriteAsync(dataToStore);
                        }
                    }
                }
                else
                {
                    QRDebug.DebugFatal(K.DebuggingChannels.SaveSystem, $"{nameof(directoryName)} should not be null.");
                }
            }
            catch (Exception e)
            {
                QRDebug.DebugFatal(K.DebuggingChannels.SaveSystem, e);
                throw;
            }
        }
    }
}