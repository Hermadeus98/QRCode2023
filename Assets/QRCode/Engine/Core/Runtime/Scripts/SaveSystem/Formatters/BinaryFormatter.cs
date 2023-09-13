namespace QRCode.Engine.Core.SaveSystem.Formatters
{
    using System;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Threading.Tasks;
    using Toolbox;
    using Engine.Debugging;
    using QRCode.Engine.Core.Tags;
    using Surrogates;
    using UnityEngine;
    using Constants = Toolbox.Constants;

    public class BinaryFormatter : IFormatter
    {
        private readonly string m_encryptionCodeWord = "372a9fcc-2639-4d91-8660-b75cd27903b0";

        public Task<T> Load<T>(string path)
        {
            if (File.Exists(path))
            {
                try
                {
                    var binaryFormatter = GetBinaryFormatter();
                    var fileStream = new FileStream(path, FileMode.Open);
                    var obj = binaryFormatter.Deserialize(fileStream);
                    var json = obj.ToString();

                    if (SaveServiceSettings.Instance.UseEncryption)
                    {
                        json = EncryptDecrypt(json);
                    }
                    
                    return Task.FromResult(JsonUtility.FromJson<T>(json));
                }
                catch (Exception e)
                {
                    QRLogger.DebugFatal<CoreTags.Save>(e);
                    throw;
                }    
            }

            return Task.FromResult<T>(default);
        }

        public Task Save(object obj, string path)
        {
            var dataToStore = JsonUtility.ToJson(obj, true);
            
            if (SaveServiceSettings.Instance.UseEncryption)
            {
                dataToStore = EncryptDecrypt(dataToStore);
            }

            var binaryFormatter = GetBinaryFormatter();
            var fileStream = new FileStream(path, FileMode.Create);
            binaryFormatter.Serialize(fileStream, dataToStore);
            fileStream.Close();
            return Task.CompletedTask;
        }

        private System.Runtime.Serialization.Formatters.Binary.BinaryFormatter GetBinaryFormatter()
        {
            var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

            var selector = new SurrogateSelector();
            var vector3SerializationSurrogate = new Vector3SerializationSurrogate();
            var quaternionSerializationSurrogate = new QuaternionSerializationSurrogate();
            
            selector.AddSurrogate(typeof(Vector3), new StreamingContext(StreamingContextStates.All), vector3SerializationSurrogate);
            selector.AddSurrogate(typeof(Quaternion), new StreamingContext(StreamingContextStates.All), quaternionSerializationSurrogate);
            
            return binaryFormatter;
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

        public void Dispose()
        {
        }
    }
}