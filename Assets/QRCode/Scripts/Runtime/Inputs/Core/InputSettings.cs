namespace QRCode.Framework
{
    using Sirenix.OdinInspector;
    using UnityEngine;

    [CreateAssetMenu(menuName = K.SettingsPath.InputSettingsPath, fileName = "Input Settings")]
    public class InputSettings : Settings<InputSettings>
    {
        [TitleGroup(K.InspectorGroups.References)]
        [SerializeField][Required] private Sprite m_notFoundedIconSprite = null;

        public Sprite NotFoundedIconSprite => m_notFoundedIconSprite;
    }
}
