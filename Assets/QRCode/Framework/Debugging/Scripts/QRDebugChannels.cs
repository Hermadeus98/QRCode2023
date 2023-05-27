namespace QRCode.Framework.Debugging
{
    using UnityEngine;
    using System;
    using System.Collections.Generic;
    using Sirenix.OdinInspector;
#if UNITY_EDITOR
    using UnityEditor;
#endif
    
    [CreateAssetMenu(menuName = K.DebuggingPath.ChannelDebugPath, fileName = "Channels Settings")]
    public class QRDebugChannels : Settings<QRDebugChannels>
    {
        #region FIELDS
        [SerializeField][TableList] private List<QRDebugChannel> channels = new List<QRDebugChannel>();
        #endregion

        #region STRUCTURES
        [Serializable]
        private struct QRDebugChannel
        {
            [ReadOnly] public string channelName;
            public bool isActive;
        }
        #endregion

        #region METHODS
        public bool ChannelIsActive(string channelName)
        {
            for (int i = 0; i < channels.Count; i++)
            {
                if (channels[i].channelName == channelName)
                {
                    return channels[i].isActive;
                }
            }

            AddChannel(channelName);
            
            return true;
        }
        
        private void AddChannel(string channelName)
        {
            channels.Add(new QRDebugChannel()
            {
                channelName = channelName,
                isActive = true
            });

#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }
        #endregion
    }
}
