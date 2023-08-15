namespace QRCode.Engine.Debugging
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using QRCode.Engine.Debugging;
    using UnityEngine;

    public class QRLogExporter
    {
        private List<string> m_allLogMessages = new List<string>();

        public const string m_goToLine = "\n";

        public QRLogExporter()
        {
            Initialize();
        }
        
        private void Initialize()
        {
            Application.logMessageReceivedThreaded += OnLogMessageReceived;    
        }

        private void OnLogMessageReceived(string logString, string stackTrace, UnityEngine.LogType type)
        {
            var dateTime = DateTime.Now;
            var timeStamp = $"[{dateTime.Hour:D2}:{dateTime.Minute:D2}:{dateTime.Second:D2}.{dateTime.Millisecond:D3}]";
            var gameTime = "[" + Time.time + "]" + "-" +"[" + Time.frameCount + "]";

            var fullMessage = $"## {timeStamp} {m_goToLine} " +
                              $"{gameTime} {m_goToLine}" +
                              $" {type} - {logString} {m_goToLine} " +
                              $"{stackTrace}";
            
            m_allLogMessages.Add(fullMessage);
        }

        public async void ExportLogFile()
        {
            var setting = QRDebugChannels.Instance;
            var dateTime = DateTime.Now;
            var timeStamp = $"{dateTime.Hour:D2}{dateTime.Minute:D2}{dateTime.Second:D2}";
            var name = timeStamp + "_Log";
            var path = Application.persistentDataPath + "/" + setting.Path;
            var fullPath = path + "/" + name + ".txt";

            var fullText = new StringBuilder();
            for (int i = 0; i < m_allLogMessages.Count; i++)
            {
                fullText.Append(m_allLogMessages[i]);
                fullText.Append(m_goToLine);
                fullText.Append(m_goToLine);
            }

            if(Directory.Exists(path) == false)
            {
                Directory.CreateDirectory(path);
            }

            using (var file = File.Open(fullPath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                using (var writer = new StreamWriter(file))
                {
                    await writer.WriteAsync(fullText.ToString());
                }
            }

            Application.quitting -= ExportLogFile;
        }
    }
}