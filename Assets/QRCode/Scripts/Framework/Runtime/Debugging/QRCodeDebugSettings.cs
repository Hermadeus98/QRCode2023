namespace QRCode.Framework
{
    using Debugging;
    using Sirenix.OdinInspector;
    using UnityEngine;

    [CreateAssetMenu(fileName = "Channels Settings", menuName = "QRCode/Debugging/Channels Settings Handler")]
    public class QRCodeDebugSettings : Settings<QRCodeDebugSettings>
    {
        [TitleGroup(K.InspectorGroups.Settings)]
        [SerializeField][InlineEditor(InlineEditorModes.FullEditor)] private QRDebugChannels m_debugChannels = null;

        public QRDebugChannels DebugChannels => m_debugChannels;

        private void OnEnable()
        {
            m_debugChannels = QRDebugChannels.Instance;
        }
    }
}
