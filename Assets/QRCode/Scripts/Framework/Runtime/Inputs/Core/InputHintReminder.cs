namespace QRCode.Framework
{
    using System;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UnityEngine.AddressableAssets;
    using UnityEngine.Localization.Components;

    public class InputHintReminder : MonoBehaviour
    {
        [TitleGroup(K.InspectorGroups.References)]
        [SerializeField] private InputHint m_inputHint;

        [TitleGroup(K.InspectorGroups.References)]
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
