namespace QRCode.Framework.Core
{
    using Debugging;
    using UnityEngine;

    public static class Bootstrapper 
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Initialize()
        {
            QRDebug.Debug(K.DebugChannels.LifeCycle, $"Bootstrapper has been initialized.");
        }
    }
}
