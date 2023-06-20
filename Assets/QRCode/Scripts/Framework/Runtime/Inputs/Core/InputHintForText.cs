namespace QRCode.Framework
{
    using Sirenix.OdinInspector;
    using TMPro;
    using UnityEngine;
    using UnityEngine.Localization.Components;
    using UnityEngine.Scripting;

    public class InputHintForText : InputHintBase
    {
        [TitleGroup(K.InspectorGroups.References)] [SerializeField] [Required]
        private TextMeshProUGUI m_text = null;

        [TitleGroup(K.InspectorGroups.References)] [SerializeField] [Required]
        private LocalizeStringEvent m_localizeStringEvent = null;
        
        [TitleGroup(K.InspectorGroups.Debugging)]
        [SerializeField][ReadOnly] private string m_output;
        
        [Preserve]
        public string Value => m_output;
        
        protected override void LoadIcon()
        {
            base.LoadIcon();

            var sanitizeControlScheme = CurrentControlScheme.Replace(" ", "");
            InputMapDatabase.TryGetInDatabase(sanitizeControlScheme, out var inputMap);
            var index = inputMap.FindTextMeshProSpriteSheetIndex(m_currentDisplayName);
            
            if (index == -1)
            {
                m_output = $"Cannot find input in database.";
            }
            else
            {
                m_output = $"<sprite={index}>";
            }
            
            var iconsSpriteAsset = inputMap.IconsSpriteAsset;
            m_text.spriteAsset = iconsSpriteAsset;
            m_text.ForceMeshUpdate();
            m_localizeStringEvent.RefreshString();
        }
    }
}
