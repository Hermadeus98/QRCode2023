namespace QRCode.Engine.Game.UI
{
    using GameConfigs;
    using Toolbox;
    using Sirenix.OdinInspector;
    using TMPro;
    using Toolbox.GameConfigs;
    using UnityEngine;

    public class TextColorComponent : MonoBehaviour
    {
        [TitleGroup(Constants.InspectorGroups.References)]
        [SerializeField] private TextMeshProUGUI m_textMeshProUGUI = null;
        
        [TitleGroup(Constants.InspectorGroups.Settings)] 
        [SerializeField] private string m_textRuleSetName = "Default";

        private void Start()
        {
            m_textMeshProUGUI.color = GetColor();
        }

        private Color GetColor()
        {
            var textRuleSetCatalog = GameConfigs.Instance.GetCatalogOfType<TextGameConfig>();
            var textRuleSet = textRuleSetCatalog.GetDataFromId(m_textRuleSetName);
            return textRuleSet.TextColor;
        }
    }
}