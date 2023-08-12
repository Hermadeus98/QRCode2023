namespace QRCode.Engine.Game.Inputs
{
    using System;
    using Toolbox;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UnityEngine.AddressableAssets;
    using UnityEngine.Localization.Components;

    public class InputHintReminder : MonoBehaviour
    {
        [TitleGroup(Constants.InspectorGroups.References)]
        [SerializeField] private InputHint m_inputHint;

        [TitleGroup(Constants.InspectorGroups.References)]
        [SerializeField] private LocalizeStringEvent m_inputHintText;
    }
    
    [Serializable]
    public class AssetReferenceInputHintReminder : AssetReferenceT<InputHintReminder>
    {
        public AssetReferenceInputHintReminder(string guid) : base(guid)
        {
        }
    }
}
