namespace QRCode.Engine
{
    using UnityEngine;

#if UNITY_EDITOR
    using UnityEditor;
#endif

    using Sirenix.OdinInspector;
    
    using Unity.Services.RemoteConfig;

    public abstract class RemoteConfigValueBase : ScriptableObject
    {
        [TitleGroup(Toolbox.Constants.InspectorGroups.Settings)] 
        [SerializeField][OnValueChanged("SetRemoteConfigNameInEditor")] protected string m_key;

        public abstract void FetchValue(RuntimeConfig runtimeConfig);
        
        private void SetRemoteConfigNameInEditor()
        {
#if UNITY_EDITOR
            var path = AssetDatabase.GetAssetPath(this);
            AssetDatabase.RenameAsset(path,  "GCV_" + m_key);
#endif
        }
    }
    
    public abstract class RemoteConfigValueBase<T> : RemoteConfigValueBase
    {
        [TitleGroup(Toolbox.Constants.InspectorGroups.Infos)]
        [ShowInInspector] [ReadOnly] protected T m_value;
        [SerializeField] protected T m_defaultValue;

        /// <summary>
        /// Get a value initialized from remote config service via a key.
        /// If the service cannot be fetch, the <see cref="m_defaultValue"/> will be set as <see cref="Value"/>.
        /// </summary>
        public T Value => m_value;

        public abstract override void FetchValue(RuntimeConfig runtimeConfig);
    }
}
