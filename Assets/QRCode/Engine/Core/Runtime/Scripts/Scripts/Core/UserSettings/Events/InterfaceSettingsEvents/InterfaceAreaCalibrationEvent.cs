namespace QRCode.Engine.Core.UserSettings.Events.InterfaceSettings
{
    using UnityEngine;

    public struct InterfaceAreaCalibrationEvent
    {
        private static event Delegate OnEvent;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)] 
        private static void RuntimeInitialization() { OnEvent = null; }
        
        public static void Register(Delegate callback) { OnEvent += callback; }
        public static void Unregister(Delegate callback) { OnEvent -= callback; }

        public delegate void Delegate(int interfaceAreaCalibrationSize);

        public static void Trigger(int interfaceAreaCalibrationSize)
        {
            OnEvent?.Invoke(interfaceAreaCalibrationSize);
        }
    }
}