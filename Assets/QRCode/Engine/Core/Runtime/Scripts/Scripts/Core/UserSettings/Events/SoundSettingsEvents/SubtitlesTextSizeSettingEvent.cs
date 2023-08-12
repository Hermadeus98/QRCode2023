namespace QRCode.Engine.Core.UserSettings.Events.SoundSettings
{
    using Settings.InterfaceSettings;
    using UnityEngine;

    public struct SubtitlesTextSizeSettingEvent
    {
        private static event Delegate OnEvent;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)] 
        private static void RuntimeInitialization() { OnEvent = null; }
        
        public static void Register(Delegate callback) { OnEvent += callback; }
        public static void Unregister(Delegate callback) { OnEvent -= callback; }

        public delegate void Delegate(TextSizeSetting textSizeSetting);

        public static void Trigger(TextSizeSetting textSizeSetting)
        {
            OnEvent?.Invoke(textSizeSetting);
        }
    }
}