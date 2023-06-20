namespace QRCode.Framework
{
    using Sirenix.OdinInspector;
    using UnityEngine;

    [CreateAssetMenu(menuName = K.SettingsPath.InterfaceSettings, fileName = "STG_InterfaceSettings")]
    public class InterfaceSettings : Settings<InterfaceSettings>
    {
        [TitleGroup("Interface Settings")]
        [SerializeField] private AnimationCurve m_menuHoldFactorProgressionCurve = null;

        public AnimationCurve MenuHoldFactorProgressionCurve
        {
            get => m_menuHoldFactorProgressionCurve;
        }
    }
}
