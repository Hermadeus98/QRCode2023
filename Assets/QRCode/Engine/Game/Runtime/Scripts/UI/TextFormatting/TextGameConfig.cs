namespace QRCode.Engine.Game.GameConfigs
{
    using System;
    using Toolbox;
    using Sirenix.OdinInspector;
    using Toolbox.GameConfigs;
    using UnityEngine;

    [CreateAssetMenu(menuName = Constants.GameConfigs.BasePath + "UI/Text Game Config", fileName = "New Text Game Config")]
    public class TextGameConfig : GameConfigBase<TextRuleSet>
    {
        
    }

    [Serializable]
    public class TextRuleSet : GameConfigDataBase
    {
        [SerializeField] private TextSizeSettingsData[] m_textSizeSettings;

        [SerializeField][ColorPalette("UI")] private UnityEngine.Color m_textColor;

        public TextSizeSettingsData GetTextSetting(Engine.Core.UserSettings.Settings.InterfaceSettings.TextSizeSetting textSizeSetting)
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
        [SerializeField] private Engine.Core.UserSettings.Settings.InterfaceSettings.TextSizeSetting m_textSizeSetting;
        [SerializeField] [Min(0)] [SuffixLabel("px")] private int m_fontSize;

        public Engine.Core.UserSettings.Settings.InterfaceSettings.TextSizeSetting TextSizeSetting => m_textSizeSetting;
        public int FontSize => m_fontSize;
    }

    [Serializable]
    public struct TextSizeSetting
    {
        [SuffixLabel("px")]
        [SerializeField] private int m_fontSize;
    }
}
