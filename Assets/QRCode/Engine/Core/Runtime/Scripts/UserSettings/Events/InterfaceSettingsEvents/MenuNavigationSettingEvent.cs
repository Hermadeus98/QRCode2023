namespace QRCode.Engine.Core.UserSettings.Events.InterfaceSettings
{
    using Settings.InterfaceSettings;
    using UnityEngine;

    public struct MenuNavigationSettingEvent
    {
        private static event Delegate OnEvent;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)] 
        private static void RuntimeInitialization() { OnEvent = null; }
        
        public static void Register(Delegate callback) { OnEvent += callback; }
        public static void Unregister(Delegate callback) { OnEvent -= callback; }

        public delegate void Delegate(MenuNavigationSettings menuNavigationSettings);

        public static void Trigger(MenuNavigationSettings menuNavigationSettings)
        {
            OnEvent?.Invoke(menuNavigationSettings);
        }
    }
}