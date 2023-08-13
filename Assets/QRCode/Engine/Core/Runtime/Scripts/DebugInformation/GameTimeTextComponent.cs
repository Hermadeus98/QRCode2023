namespace QRCode.Engine.Core.DebugInformation
{
    using TMPro;
    using UnityEngine;

    public class GameTimeTextComponent : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_textMeshProUGUI = null;

        private void Update()
        {
            m_textMeshProUGUI.SetText("t = " + Mathf.RoundToInt(Time.time).ToString());
        }

        private void OnDestroy()
        {
            m_textMeshProUGUI = null;
        }
    }
}
