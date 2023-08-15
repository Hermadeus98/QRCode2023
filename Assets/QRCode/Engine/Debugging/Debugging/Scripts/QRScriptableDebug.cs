namespace QRCode.Engine.Debugging
{
    using UnityEngine;
    using UnityEngine.Scripting;

    [CreateAssetMenu(menuName = Constants.DebuggingPath.ScriptableDebugPath, fileName = "Scriptable Object Debugger")]
    public class QRScriptableDebug : ScriptableObject
    {
        
        /// <summary>
        /// Allow a Debug Message from Unity Event for example...
        /// </summary>
        /// <param name="message"></param>
        [Preserve]
        public void Debug(string message)
        {
            QRDebug.DebugTrace("Misc", message);
        }
    }
}
