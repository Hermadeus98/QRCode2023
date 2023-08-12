namespace QRCode.Engine.Core.Inputs
{
    using Toolbox;
    using Sirenix.OdinInspector;
    using Toolbox.Settings;
    using UnityEngine;

    [CreateAssetMenu(menuName = Constants.SettingsPath.InputSettingsPath, fileName = "Input Settings")]
    public class InputSettings : Settings<InputSettings>
    {
        [TitleGroup(Constants.InspectorGroups.References)]
        [SerializeField][Required] private Sprite m_notFoundedIconSprite = null;

        public Sprite NotFoundedIconSprite => m_notFoundedIconSprite;
    }
}
