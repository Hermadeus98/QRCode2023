namespace QRCode.Framework.Debugging
{
    using System;
    using QRCode.Utils;
    using Object = UnityEngine.Object;
    
    public static class QRDebug
    {
        #region FIELDS
        private static QRDebugChannels m_debugChannels = null;
        #endregion

        #region METHODS
        public static string GetFieldDebug(object o)
        {
            return $"{nameof(o)} = {o}";
        }
        
        public static void DebugTrace(string channel, object message, Object context = null)
        {
            DebugMessage(LogType.Trace, channel, message, context);
        }
        
        public static void Debug(string channel, object message, Object context = null)
        {
            DebugMessage(LogType.Debug, channel, message, context);
        }
        
        public static void DebugInfo(string channel, object message, Object context = null)
        {
            DebugMessage(LogType.Info, channel, message, context);
        }
        
        public static void DebugWarning(string channel, object message, Object context = null)
        {
            DebugMessage(LogType.Warning, channel, message, context);
        }
        
        public static void DebugError(string channel, object message, Object context = null)
        {
            DebugMessage(LogType.Error, channel, message, context);
        }
        
        public static void DebugFatal(string channel, object message, Object context = null)
        {
            DebugMessage(LogType.Fatal, channel, message, context);
        }
        
        public static void DebugMessage(LogType logType, string channel, object message, Object context = null)
        {
            if (m_debugChannels == null)
            {
                m_debugChannels = QRDebugChannels.Instance;
            }

            if (m_debugChannels.ChannelIsActive(channel.ToUpper()))
            {
                switch (logType)
                {
                    case LogType.Trace:
                        UnityEngine.Debug.Log(
                            $"{GetColoredMessage(ColorPalletsUtils.GetHTLMCode(FlatUIPallet.Clouds), $"[{channel.ToUpper()}] -> ")} {message}",
                            context);
                        break;
                    case LogType.Debug:
                        UnityEngine.Debug.Log(
                            $"{GetColoredMessage(ColorPalletsUtils.GetHTLMCode(FlatUIPallet.Concrete), $"[{channel.ToUpper()}] -> ")} {message}",
                            context);
                        break;
                    case LogType.Info:
                        UnityEngine.Debug.Log(
                            $"{GetColoredMessage(ColorPalletsUtils.GetHTLMCode(FlatUIPallet.BelizeHole), $"[{channel.ToUpper()}] -> ")} {message}",
                            context);
                        break;
                    case LogType.Warning:
                        UnityEngine.Debug.LogWarning(
                            $"{GetColoredMessage(ColorPalletsUtils.GetHTLMCode(FlatUIPallet.Orange), $"[{channel.ToUpper()}] -> ")} {message}",
                            context);
                        break;
                    case LogType.Error:
                        UnityEngine.Debug.LogError(
                            $"{GetColoredMessage(ColorPalletsUtils.GetHTLMCode(FlatUIPallet.Pumpkin), $"[{channel.ToUpper()}] -> ")} {message}",
                            context);
                        break;
                    case LogType.Fatal:
                        UnityEngine.Debug.LogError(
                            $"{GetColoredMessage(ColorPalletsUtils.GetHTLMCode(FlatUIPallet.Pomegranate), $"[{channel.ToUpper()}] -> ")} {message}",
                            context);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(logType), logType, null);
                }
            }
        }

        private static string GetColoredMessage(string htlmCode, object message)
        {
            return $"<color={htlmCode}>{message}</color>";
        }
        #endregion
    }

    public enum LogType
    {
        /// <summary>
        /// Only when I would be "tracing" the code and trying to find one part of a function specifically.
        /// </summary>
        Trace,
        
        /// <summary>
        /// Information that is diagnostically helpful to people more than just developers (IT, sysadmins, etc.).
        /// </summary>
        Debug,
        
        /// <summary>
        /// Generally useful information to log (service start/stop, configuration assumptions, etc). Info I want to always have available but usually don't care about under normal circumstances. This is my out-of-the-box config level.
        /// </summary>
        Info,
        
        /// <summary>
        /// Anything that can potentially cause application oddities, but for which I am automatically recovering. (Such as switching from a primary to backup server, retrying an operation, missing secondary data, etc.)
        /// </summary>
        Warning,
        
        /// <summary>
        /// Any error which is fatal to the operation, but not the service or application (can't open a required file, missing data, etc.).
        /// These errors will force user (administrator, or direct user) intervention.
        /// These are usually reserved (in my apps) for incorrect connection strings, missing services, etc.
        /// </summary>
        Error,
        
        /// <summary>
        /// Any error that is forcing a shutdown of the service or application to prevent data loss (or further data loss).
        /// I reserve these only for the most heinous errors and situations where there is guaranteed to have been data corruption or loss.
        /// </summary>
        Fatal,
    }
}