namespace QRCode.Engine.Core.DebugInformation
{
    using UnityEngine;
    using UnityEngine.UI;
    
    using TMPro;

    using RemoteConfig;

    public class RemoteConfigVersionComponent : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_textMeshProUGUI = null;
        [SerializeField] private RemoteConfigStringValue m_remoteConfigVersion = null;

        private RemoteConfigManager m_remoteConfigManager = null;
        
        private void OnEnable()
        {
            m_remoteConfigManager = RemoteConfigManager.Instance;
            m_remoteConfigManager.RegisterOnValueFetched(m_remoteConfigVersion, UpdateText);
            UpdateText();
        }

        private void OnDisable()
        {
            m_remoteConfigManager.UnregisterOnValueFetched(m_remoteConfigVersion, UpdateText);
        }

        private void UpdateText()
        {
            m_textMeshProUGUI.SetText("RCVersion = " + m_remoteConfigVersion.Value);
            LayoutRebuilder.ForceRebuildLayoutImmediate(transform.parent as RectTransform);
        }
    }
}
