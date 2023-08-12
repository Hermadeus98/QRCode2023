namespace QRCode.Engine.Core.UserSettings.Events.SoundSettings
{
    using UnityEngine;

    public struct ChangeSubtitleBackgroundOpacityEvent
    {
        private static event Delegate OnEvent;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)] 
        private static void RuntimeInitialization() { OnEvent = null; }
        
        public static void Register(Delegate callback) { OnEvent += callback; }
        public static void Unregister(Delegate callback) { OnEvent -= callback; }

        public delegate void Delegate(int subtitleBackgroundOpacity);

        public static void Trigger(int subtitleBackgroundOpacity)
        {
            OnEvent?.Invoke(subtitleBackgroundOpacity);
        }
    }
}