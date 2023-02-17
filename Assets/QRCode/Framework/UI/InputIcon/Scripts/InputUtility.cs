namespace QRCode.Framework
{
    using Observer;
    using UnityEngine;

    public static class InputUtility
    {
        private static Observer.Observer<InputChangeInfo> m_inputChangeObserver;
        private static InputChangeInfo m_inputChangeInfo;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Initialize()
        {
            m_inputChangeObserver = new Observer<InputChangeInfo>();
            m_inputChangeInfo = new InputChangeInfo()
            {
                CurrentInputScheme = InputScheme.Keyboard,
                CurrentInputSchemeString = K.InputsSchemes.Keyboard,
            };
        }
    }

    public enum InputScheme
    {
        Undefined = 0,
        Keyboard = 1,
        XboxController = 2,
    }

    public struct InputChangeInfo
    {
        public InputScheme CurrentInputScheme;
        public string CurrentInputSchemeString;
    }
}
