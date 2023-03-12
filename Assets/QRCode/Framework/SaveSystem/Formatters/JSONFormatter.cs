namespace QRCode.Framework.Formatters
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Debugging;
    using UnityEngine;
    
    public class JSONFormatter : IFormatter
    {
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

                    return JsonUtility.FromJson<T>(dataToLoad);
                }
                catch (Exception e)
                {
                    QRDebug.DebugFatal(K.DebuggingChannels.SaveSystem, e);
                    throw;
                }    
            }

            return default;
        }

        public async Task Save(object obj, string path)
        {
            var dataToStore = JsonUtility.ToJson(obj, true);
            
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
                    QRDebug.DebugFatal(K.DebuggingChannels.SaveSystem, $"{nameof(directoryName)} should not be null.");
                }
            }
            catch (Exception e)
            {
                QRDebug.DebugFatal(K.DebuggingChannels.SaveSystem, e);
                throw;
            }
        }

        public Task<bool> TryDeleteFile(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                    return Task.FromResult(true);
                }
                else
                {
                    QRDebug.DebugFatal(K.DebuggingChannels.SaveSystem, $"Cannot delete any file at path {path}.");
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