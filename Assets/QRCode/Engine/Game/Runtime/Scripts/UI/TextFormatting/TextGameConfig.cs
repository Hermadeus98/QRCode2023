namespace QRCode.Engine.Game.GameConfigs
{
    using UnityEngine;

    using System;
    
    using Sirenix.OdinInspector;

    using Toolbox;
    using Toolbox.GameConfigs;

    [CreateAssetMenu(menuName = Constants.GameConfigs.BasePath + "UI/Text Game Config", fileName = "New Text Game Config")]
    public class TextGameConfig : GameConfigBase<TextRuleSet>
    {
        protected override string GameConfigDescription { get => "This Game Config must contains all text formatting of the game."; }
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
