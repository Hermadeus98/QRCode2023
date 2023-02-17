namespace QRCode.Framework.Core
{
    using Debugging;
    using UnityEngine;

    public static class Bootstrapper 
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Initialize()
        {
            QRDebug.Debug(K.DebuggingChannels.LifeCycle, $"Bootstrapper has been initialized.");
        }
    }
}
