namespace QRCode.Engine.Core.DebugInformation
{
    using UnityEngine;
    using UnityEngine.UI;
    
    using TMPro;

    public class AppVersionTextComponent : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_textMeshProUGUI = null;

        private void Awake()
        {
            var appVersion = Application.version;
            m_textMeshProUGUI.SetText("AppVersion = " + appVersion);
            LayoutRebuilder.ForceRebuildLayoutImmediate(transform.parent as RectTransform);
        }
    }
}
