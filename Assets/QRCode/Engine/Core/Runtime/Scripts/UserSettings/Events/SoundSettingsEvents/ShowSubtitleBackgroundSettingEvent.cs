namespace QRCode.Engine.Core.UserSettings.Events.SoundSettings
{
    using UnityEngine;

    public struct ShowSubtitleBackgroundSettingEvent
    {
        private static event Delegate OnEvent;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)] 
        private static void RuntimeInitialization() { OnEvent = null; }
        
        public static void Register(Delegate callback) { OnEvent += callback; }
        public static void Unregister(Delegate callback) { OnEvent -= callback; }

        public delegate void Delegate(bool showSubtitleBackground);

        public static void Trigger(bool showSubtitleBackground)
        {
            OnEvent?.Invoke(showSubtitleBackground);
        }
    }
}