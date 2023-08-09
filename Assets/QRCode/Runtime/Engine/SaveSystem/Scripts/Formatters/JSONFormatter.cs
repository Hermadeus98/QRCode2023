namespace QRCode.Framework.Formatters
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Debugging;
    using UnityEngine;
    using K = Framework.K;

    public class JSONFormatter : IFormatter
    {
        private readonly string m_encryptionCodeWord = "372a9fcc-2639-4d91-8660-b75cd27903b0";

        public async Task<T> Load<T>(string path)
        {
            if (File.Exists(path))
            {
                try
                {
                    var dataToLoad = "";
                    using (var fileStream = new FileStream(path, FileMode.Open))
                    {
                        using (var streamReader = new StreamReader(fileStream))
                        {
                            dataToLoad = await streamReader.ReadToEndAsync();
                        }
                    }

                    if (SaveServiceSettings.Instance.UseEncryption)
                    {
                        dataToLoad = EncryptDecrypt(dataToLoad);
                    }
                    
                    return JsonUtility.FromJson<T>(dataToLoad);
                }
                catch (Exception e)
                {
                    QRDebug.DebugFatal(K.DebuggingChannels.SaveManager, e);
                    throw;
                }    
            }

            return default;
        }

        public async Task Save(object obj, string path)
        {
            var dataToStore = JsonUtility.ToJson(obj, true);

            if (SaveServiceSettings.Instance.UseEncryption)
            {
                dataToStore = EncryptDecrypt(dataToStore);
            }
            
            try
            {
                var directoryName = Path.GetDirectoryName(path);
                if (directoryName != null)
                {
                    Directory.CreateDirectory(directoryName);

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        using (var streamWriter = new StreamWriter(fileStream))
                        {
                            await streamWriter.WriteAsync(dataToStore);
                        }
                    }
                }
                else
                {
                    QRDebug.DebugFatal(K.DebuggingChannels.SaveManager, $"{nameof(directoryName)} should not be null.");
                }
            }
            catch (Exception e)
            {
                QRDebug.DebugFatal(K.DebuggingChannels.SaveManager, e);
                throw;
            }
        }
        
        private string EncryptDecrypt(string data)
        {
            var modifiedData = "";
            for (var i = 0; i < data.Length; i++)
            {
                modifiedData += (char)(data[i] ^ m_encryptionCodeWord[i % m_encryptionCodeWord.Length]);
            }

            return modifiedData;
        }
    }
}