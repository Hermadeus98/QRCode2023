namespace QRCode.Framework.Debugging
{
    using UnityEngine;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Sirenix.OdinInspector;
#if UNITY_EDITOR
    using UnityEditor;
#endif
    
    [CreateAssetMenu(menuName = K.DebuggingPath.ChannelDebugPath, fileName = "Channels Settings")]
    public class QRDebugChannels : SerializedScriptableObject
    {
        private static QRDebugChannels m_instance;

        public static QRDebugChannels Instance
        {
            get
            {
                if (!m_instance)
                {
                    m_instance = Resources.LoadAll<QRDebugChannels>("").FirstOrDefault();
                }
                if (!m_instance) throw new Exception($"Cannot find instance of {typeof(QRDebugChannels)} in Resources.");
                if(m_instance) m_instance.hideFlags = HideFlags.DontUnloadUnusedAsset;
                return m_instance;
            }
        }
        
        #region FIELDS
        [SerializeField][TableList] private List<QRDebugChannel> channels = new List<QRDebugChannel>();
        
        [ToggleGroup("m_exportLog", "Export Log")]
        [SerializeField] private bool m_exportLog = false;

        [SerializeField] [ToggleGroup("m_exportLog", "Export Log")]
        [Sirenix.OdinInspector.FilePath] private string m_path = "QRCodeLog";
        
        public bool UseExportLogOption => m_exportLog;
        public string Path => m_path;
        #endregion

        #region STRUCTURES
        [Serializable]
        public struct QRDebugChannel
        {
            [ReadOnly] public string channelName;
            public bool isActive;
            [ColorPalette("Debug Channels")] public Color channelColor;
            public LogType activeLogTypes;

            public QRDebugChannel(string channelName)
            {
                this.channelName = channelName;
                isActive = true;
                channelColor = Color.white;
                activeLogTypes = LogType.Debug | LogType.Error | LogType.Fatal | LogType.Info | LogType.Trace | LogType.Warning;
            }
        }
        #endregion

        #region METHODS
        public bool ChannelIsActive(string channelName, out QRDebugChannel outChannel)
        {
            for (var i = 0; i < channels.Count; i++)
            {
                if (channels[i].channelName == channelName)
                {
                    outChannel = channels[i];
                    return channels[i].isActive;
                }
            }

            var newDebugChannel = AddChannel(channelName);
            outChannel = newDebugChannel;
            return true;
        }
        
        private QRDebugChannel AddChannel(string channelName)
        {
            var newDebugChannel = new QRDebugChannel(channelName)
            {
                isActive = true
            };

            channels.Add(newDebugChannel);

#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
            return newDebugChannel;
        }
        #endregion
    }
}
