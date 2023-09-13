namespace QRCode.Engine.Core.SaveSystem
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Formatters;
    using Toolbox;
    using Engine.Debugging;
    using QRCode.Engine.Core.Tags;
    using Constants = Toolbox.Constants;

    /// <summary>
    /// Default File Data Handler, used for PC save and Editor Save.
    /// </summary>
    public class FileDataHandler : IFileDataHandler
    {
        private readonly string m_fullPath = String.Empty;
        private readonly IFormatter m_formatter = null;
        
        public FileDataHandler(string dataDirectoryPath, string dataFileName)
        {
            m_fullPath = Path.Combine(dataDirectoryPath, dataFileName);
            m_formatter = FormatterFactory.CreateFormatter(SaveServiceSettings.Instance.FormatterTypeDefault);
        }

        public async Task<T> Load<T>()
        {
            var loadedObject = await m_formatter.Load<T>(m_fullPath);

            return loadedObject;
        }
        
        public async Task Save(object saveData)
        {
            await m_formatter.Save(saveData, m_fullPath);
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
                    QRLogger.DebugFatal<CoreTags.Save>($"Cannot delete any file at path {m_fullPath}.");
                }
            }
            catch (Exception e)
            {
                QRLogger.DebugFatal<CoreTags.Save>(e);
                throw;
            }

            return Task.FromResult(false);
        }

        public void Dispose()
        {
            if (m_formatter != null)
            {
                m_formatter.Dispose();
            }
        }
    }
}