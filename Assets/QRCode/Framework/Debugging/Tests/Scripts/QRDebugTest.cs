namespace QRCode.Framework.Debugging.Tests
{
    using UnityEngine;
    using LogType = QRCode.Framework.Debugging.LogType;

    public class QRDebugTest : MonoBehaviour
    {
        private void Start()
        {
            QRDebug.DebugMessage(LogType.Error, "Error", $"Im an error log message.", gameObject);
            QRDebug.DebugMessage(LogType.Trace, "Trace", $"Im a trace log message.", gameObject);
            QRDebug.DebugMessage(LogType.Fatal, "Fatal", $"Im a fatal log message.", gameObject);
            QRDebug.DebugMessage(LogType.Debug, "Debug", $"Im a debug log message.", gameObject);
            QRDebug.DebugMessage(LogType.Info, "Info", $"Im an info log message.", gameObject);
            QRDebug.DebugMessage(LogType.Warning, "Warning", $"Im a warning log message.");
        }
    }
}
