namespace QRCode.Framework
{
    using Sirenix.OdinInspector;
    using TMPro;
    using UnityEngine;

    public class TextColorComponent : MonoBehaviour
    {
        [TitleGroup(K.InspectorGroups.References)]
        [SerializeField] private TextMeshProUGUI m_textMeshProUGUI = null;
        
        [TitleGroup(K.InspectorGroups.Settings)] 
        [SerializeField] private string m_textRuleSetName = "Default";

        private void Start()
        {
            m_textMeshProUGUI.color = GetColor();
        }

        private Color GetColor()
        {
            var textRuleSetCatalog = Catalog.Instance.GetCatalogOfType<TextRuleSetCatalog>();
            var textRuleSet = textRuleSetCatalog.GetDataFromId(m_textRuleSetName);
            return textRuleSet.TextColor;
        }
    }
}