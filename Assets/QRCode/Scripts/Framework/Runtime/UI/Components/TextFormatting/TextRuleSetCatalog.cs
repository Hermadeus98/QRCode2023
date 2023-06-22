namespace QRCode.Framework
{
    using System;
    using Sirenix.OdinInspector;
    using UnityEngine;

    [CreateAssetMenu(menuName = K.Catalog.BasePath + "UI/TextRuleSet", fileName = "New Text Rule Set")]
    public class TextRuleSetCatalog : CatalogObject<TextRuleSet>
    {
        
    }

    [Serializable]
    public class TextRuleSet : CatalogDataBase
    {
        [SerializeField] private TextSizeSettingsData[] m_textSizeSettings;

        [SerializeField][ColorPalette("UI")] private UnityEngine.Color m_textColor;

        public TextSizeSettingsData GetTextSetting(Settings.InterfaceSettings.TextSizeSetting textSizeSetting)
        {
            for (var i = 0; i < m_textSizeSettings.Length; i++)
            {
                if (m_textSizeSettings[i].TextSizeSetting == textSizeSetting)
                {
                    return m_textSizeSettings[i];
                }
            }

            throw new Exception();
        }
        public UnityEngine.Color TextColor => m_textColor;
    }

    [Serializable]
    public struct TextSizeSettingsData
    {
        [SerializeField] private Settings.InterfaceSettings.TextSizeSetting m_textSizeSetting;
        [SerializeField] [Min(0)] [SuffixLabel("px")] private int m_fontSize;

        public Settings.InterfaceSettings.TextSizeSetting TextSizeSetting => m_textSizeSetting;
        public int FontSize => m_fontSize;
    }

    [Serializable]
    public struct TextSizeSetting
    {
        [SuffixLabel("px")]
        [SerializeField] private int m_fontSize;
    }
}
