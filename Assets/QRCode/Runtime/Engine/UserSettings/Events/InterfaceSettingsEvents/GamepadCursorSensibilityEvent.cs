namespace QRCode.Framework.Events
{
    using UnityEngine;

    public struct GamepadCursorSensibilityEvent
    {
        private static event Delegate OnEvent;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)] 
        private static void RuntimeInitialization() { OnEvent = null; }
        
        public static void Register(Delegate callback) { OnEvent += callback; }
        public static void Unregister(Delegate callback) { OnEvent -= callback; }

        public delegate void Delegate(int gamepadCursorSensibility);

        public static void Trigger(int gamepadCursorSensibility)
        {
            OnEvent?.Invoke(gamepadCursorSensibility);
        }
    }
}