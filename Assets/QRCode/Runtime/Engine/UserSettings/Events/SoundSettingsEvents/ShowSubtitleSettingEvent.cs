namespace QRCode.Framework.Events
{
    using UnityEngine;

    public struct ShowSubtitleSettingEvent
    {
        private static event Delegate OnEvent;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)] 
        private static void RuntimeInitialization() { OnEvent = null; }
        
        public static void Register(Delegate callback) { OnEvent += callback; }
        public static void Unregister(Delegate callback) { OnEvent -= callback; }

        public delegate void Delegate(bool showSubtitles);

        public static void Trigger(bool showSubtitles)
        {
            OnEvent?.Invoke(showSubtitles);
        }
    }
}