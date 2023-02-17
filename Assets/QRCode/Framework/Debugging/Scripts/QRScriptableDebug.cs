namespace QRCode.Framework.Debugging
{
    using UnityEngine;

    [CreateAssetMenu(menuName = K.DebuggingPath.ScriptableDebugPath, fileName = "Scriptable Object Debugger")]
    public class QRScriptableDebug : ScriptableObject
    {
        public void Debug(string message)
        {
            QRDebug.DebugTrace(K.DebuggingChannels.Misc, message);
        }
    }
}
