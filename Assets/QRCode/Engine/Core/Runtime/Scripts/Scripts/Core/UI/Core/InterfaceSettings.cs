namespace QRCode.Engine.Core.UI
{
    using Toolbox;
    using Sirenix.OdinInspector;
    using Toolbox.Settings;
    using UnityEngine;

    [CreateAssetMenu(menuName = Constants.SettingsPath.InterfaceSettings, fileName = "STG_InterfaceSettings")]
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
