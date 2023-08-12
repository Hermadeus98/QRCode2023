namespace QRCode.Engine.Game.Inputs
{
    using System;
    using Toolbox;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UnityEngine.AddressableAssets;

    [Serializable]
    public class InputHintReminderBatch
    {
        [TitleGroup(Constants.InspectorGroups.References)]
        [SerializeField][DrawWithUnity] private AssetReference[] m_assetReferenceInputHintReminders;

        public AssetReference[] AssetReferenceInputHintReminders => m_assetReferenceInputHintReminders;
    }
}
