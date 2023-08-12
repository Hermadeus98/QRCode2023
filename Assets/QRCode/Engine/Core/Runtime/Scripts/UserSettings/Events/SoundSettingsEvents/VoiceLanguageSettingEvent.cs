namespace QRCode.Engine.Core.UserSettings.Events.SoundSettings
{
    using UnityEngine;
    using UnityEngine.Localization;

    public struct VoiceLanguageSettingEvent
    {
        private static event Delegate OnEvent;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)] 
        private static void RuntimeInitialization() { OnEvent = null; }
        
        public static void Register(Delegate callback) { OnEvent += callback; }
        public static void Unregister(Delegate callback) { OnEvent -= callback; }

        public delegate void Delegate(LocaleIdentifier audioLocaleIdentifier);

        public static void Trigger(LocaleIdentifier audioLocaleIdentifier)
        {
            OnEvent?.Invoke(audioLocaleIdentifier);
        }
    }
}