namespace QRCode.Framework
{
    using System;
    using Settings.InterfaceSettings;
    using Sirenix.OdinInspector;
    using UnityEngine;

    [CreateAssetMenu(menuName = K.Catalog.BasePath + "UI/TextRuleSet", fileName = "New Text Rule Set")]
    public class TextRuleSetCatalog : CatalogObject<TextRuleSet>
    {
        
    }

    [Serializable]
    public class TextRuleSet : CatalogDataBase
    {
        [SerializeField] private TextSettings[] m_textSettings;

        public TextSettings GetTextSetting(TextSizeSetting textSizeSetting)
        {
            for (var i = 0; i < m_textSettings.Length; i++)
            {
                if (m_textSettings[i].TextSizeSetting == textSizeSetting)
                {
                    return m_textSettings[i];
                }
            }

            throw new Exception();
        }
    }

    [Serializable]
    public struct TextSettings
    {
        [SerializeField] private TextSizeSetting m_textSizeSetting;
        [SerializeField] [Min(0)] [SuffixLabel("px")] private int m_fontSize;

        public TextSizeSetting TextSizeSetting => m_textSizeSetting;
        public int FontSize => m_fontSize;
    }

    [Serializable]
    public struct TextSetting
    {
        [SuffixLabel("px")]
        [SerializeField] private int m_fontSize;
    }
}
