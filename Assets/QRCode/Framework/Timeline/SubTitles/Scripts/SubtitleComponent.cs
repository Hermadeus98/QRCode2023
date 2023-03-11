namespace QRCode.Framework
{
    using Sirenix.OdinInspector;
    using TMPro;
    using UnityEngine;

    public class SubtitleComponent : SerializedMonoBehaviour
    {
        [TitleGroup(K.InspectorGroups.References)] [SerializeField]
        private TextMeshProUGUI m_subtitleText = null;

        [TitleGroup(K.InspectorGroups.References)] [SerializeField]
        private CanvasGroup m_subtitleCanvasGroup = null;
        
        public TextMeshProUGUI SubtitleText => m_subtitleText;
        public CanvasGroup SubtitleCanvasGroup => m_subtitleCanvasGroup;

        public void SetTextRaw(string text)
        {
            m_subtitleText.SetText(text);
        }
    }
}
