namespace QRCode.Framework
{
    using System;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UnityEngine.AddressableAssets;

    [Serializable]
    public class InputHintReminderBatch
    {
        [TitleGroup(K.InspectorGroups.References)]
        [SerializeField][DrawWithUnity] private AssetReference[] m_assetReferenceInputHintReminders;

        public AssetReference[] AssetReferenceInputHintReminders => m_assetReferenceInputHintReminders;
    }
}
