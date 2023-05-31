namespace QRCode.Framework.Debugging
{
    using System;
    using UnityEngine;
    using Object = UnityEngine.Object;
    
    public static class QRDebug
    {
        #region FIELDS
        private static QRDebugChannels m_debugChannels = null;
        #endregion

        #region METHODS
        
        /// <summary>
        /// Only when I would be "tracing" the code and trying to find one part of a function specifically.
        /// </summary>
        public static void DebugTrace(string channel, object message, Object context = null)
        {
            DebugMessage(LogType.Trace, channel, message, context);
        }
        
        /// <summary>
        /// Information that is diagnostically helpful to people more than just developers (IT, sysadmins, etc.).
        /// </summary>
        public static void Debug(string channel, object message, Object context = null)
        {
            DebugMessage(LogType.Debug, channel, message, context);
        }
        
        /// <summary>
        /// Generally useful information to log (service start/stop, configuration assumptions, etc). Info I want to always have available but usually don't care about under normal circumstances. This is my out-of-the-box config level.
        /// </summary>
        public static void DebugInfo(string channel, object message, Object context = null)
        {
            DebugMessage(LogType.Info, channel, message, context);
        }
        
        /// <summary>
        /// Anything that can potentially cause application oddities, but for which I am automatically recovering. (Such as switching from a primary to backup server, retrying an operation, missing secondary data, etc.)
        /// </summary>
        public static void DebugWarning(string channel, object message, Object context = null)
        {
            DebugMessage(LogType.Warning, channel, message, context);
        }
        
        /// <summary>
        /// Any error which is fatal to the operation, but not the service or application (can't open a required file, missing data, etc.).
        /// These errors will force user (administrator, or direct user) intervention.
        /// These are usually reserved (in my apps) for incorrect connection strings, missing services, etc.
        /// </summary>
        public static void DebugError(string channel, object message, Object context = null)
        {
            DebugMessage(LogType.Error, channel, message, context);
        }
        
        /// <summary>
        /// Any error that is forcing a shutdown of the service or application to prevent data loss (or further data loss).
        /// I reserve these only for the most heinous errors and situations where there is guaranteed to have been data corruption or loss.
        /// </summary>
        public static void DebugFatal(string channel, object message, Object context = null)
        {
            DebugMessage(LogType.Fatal, channel, message, context);
        }
        
        public static void DebugMessage(LogType logType, string channel, object message, Object context = null)
        {
#if USE_QRDEBUG
            if (m_debugChannels == null)
            {
                m_debugChannels = QRDebugChannels.Instance;
            }

            if (m_debugChannels.ChannelIsActive(channel.ToUpper(), out var debugChannel))
            {
                var color =  "#" + ColorUtility.ToHtmlStringRGBA(debugChannel.channelColor);
                
                switch (logType)
                {
                    case LogType.Trace:
                        if(debugChannel.activeLogTypes.HasFlag(LogType.Trace))
                        {
                            UnityEngine.Debug.Log(
                                $"{GetColoredMessage(color, $"[{channel.ToUpper()}] -> ")} {message}",
                                context);
                        }
                        break;
                    case LogType.Debug:
                        if(debugChannel.activeLogTypes.HasFlag(LogType.Debug))
                        {
                            UnityEngine.Debug.Log(
                                $"{GetColoredMessage(color, $"[{channel.ToUpper()}] -> ")} {message}",
                                context);
                        }
                        break;
                    case LogType.Info:
                        if(debugChannel.activeLogTypes.HasFlag(LogType.Info))
                        {
                            UnityEngine.Debug.Log(
                                $"{GetColoredMessage(color, $"[{channel.ToUpper()}] -> ")} {message}",
                                context);
                        }
                        break;
                    case LogType.Warning:
                        if(debugChannel.activeLogTypes.HasFlag(LogType.Warning))
                        {
                            UnityEngine.Debug.LogWarning(
                                $"{GetColoredMessage(color, $"[{channel.ToUpper()}] -> ")} {message}",
                                context);
                        }
                        break;
                    case LogType.Error:
                        if(debugChannel.activeLogTypes.HasFlag(LogType.Error))
                        {
                            UnityEngine.Debug.LogError(
                                $"{GetColoredMessage(color, $"[{channel.ToUpper()}] -> ")} {message}",
                                context);
                        }
                        break;
                    case LogType.Fatal:
                        if(debugChannel.activeLogTypes.HasFlag(LogType.Fatal))
                        {
                            UnityEngine.Debug.LogError(
                                $"{GetColoredMessage(color, $"[{channel.ToUpper()}] -> ")} {message}",
                                context);
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(logType), logType, null);
                }
            }
#endif
        }

        private static string GetColoredMessage(string htlmCode, object message)
        {
            return $"<b> <color={htlmCode}>{message}</color> </b>";
        }
        #endregion
    }

    [Flags]
    public enum LogType
    {
        Null = 0,
        
        /// <summary>
        /// Only when I would be "tracing" the code and trying to find one part of a function specifically.
        /// </summary>
        Trace = 1,
        
        /// <summary>
        /// Information that is diagnostically helpful to people more than just developers (IT, sysadmins, etc.).
        /// </summary>
        Debug = 2,
        
        /// <summary>
        /// Generally useful information to log (service start/stop, configuration assumptions, etc). Info I want to always have available but usually don't care about under normal circumstances. This is my out-of-the-box config level.
        /// </summary>
        Info = 4,
        
        /// <summary>
        /// Anything that can potentially cause application oddities, but for which I am automatically recovering. (Such as switching from a primary to backup server, retrying an operation, missing secondary data, etc.)
        /// </summary>
        Warning = 8,
        
        /// <summary>
        /// Any error which is fatal to the operation, but not the service or application (can't open a required file, missing data, etc.).
        /// These errors will force user (administrator, or direct user) intervention.
        /// These are usually reserved (in my apps) for incorrect connection strings, missing services, etc.
        /// </summary>
        Error = 16,
        
        /// <summary>
        /// Any error that is forcing a shutdown of the service or application to prevent data loss (or further data loss).
        /// I reserve these only for the most heinous errors and situations where there is guaranteed to have been data corruption or loss.
        /// </summary>
        Fatal = 32,
    }
}