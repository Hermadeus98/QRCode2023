namespace QRCode.Engine.Debugging
{
    using Toolbox;
    using Sirenix.OdinInspector;
    using Toolbox.Settings;
    using UnityEngine;

    [CreateAssetMenu(fileName = "Channels Settings", menuName = "QRCode/Debugging/Channels Settings Handler")]
    public class QRCodeDebugSettings : Settings<QRCodeDebugSettings>
    {
        [TitleGroup(Toolbox.Constants.InspectorGroups.Settings)]
        [SerializeField][InlineEditor(InlineEditorModes.FullEditor)] private QRDebugChannels m_debugChannels = null;

        public QRDebugChannels DebugChannels => m_debugChannels;

        private void OnEnable()
        {
            m_debugChannels = QRDebugChannels.Instance;
        }
    }
}
