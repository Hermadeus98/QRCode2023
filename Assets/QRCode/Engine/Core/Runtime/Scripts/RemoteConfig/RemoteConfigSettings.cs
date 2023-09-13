namespace QRCode.Engine.Core.RemoteConfig
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    
    using UnityEngine;
#if UNITY_EDITOR
    using UnityEditor;
#endif

    using Sirenix.OdinInspector;
    
    using Toolbox;
    using Toolbox.Settings;
    using Object = UnityEngine.Object;

    [CreateAssetMenu(menuName = Constants.SettingsPath.RemoteConfigSettings, fileName = "STG_RemoteConfig")]
    public class RemoteConfigSettings : Settings<RemoteConfigSettings>
    {
        [TitleGroup(Toolbox.Constants.InspectorGroups.References)] 
        [SerializeField] private RemoteConfigStringValue m_appVersion = null;
        
        [TitleGroup(Toolbox.Constants.InspectorGroups.Settings)]
        private string m_environmentId = string.Empty;

        [TitleGroup(Toolbox.Constants.InspectorGroups.Settings)]
        [SerializeField] private RemoteConfigValueBase[] m_allRemoteConfigValues = null;

        public string EnvironmentId => m_environmentId;
        public RemoteConfigValueBase[] AllRemoteConfigValues => m_allRemoteConfigValues;

        public string AppVersion => m_appVersion.Value;

        [Button]
        private void Fetch()
        {
#if UNITY_EDITOR
            var catalogs = FindAssetsByType<RemoteConfigValueBase>();
            m_allRemoteConfigValues = catalogs as RemoteConfigValueBase[] ?? catalogs.ToArray();
            EditorUtility.SetDirty(this);
#endif
        }
        
        private static IEnumerable<T> FindAssetsByType<T>() where T : Object 
        {
            var guids = AssetDatabase.FindAssets($"t:{typeof(T)}");
            foreach (var t in guids) 
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(t);
                var asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
                if (asset != null) 
                {
                    yield return asset;
                }
            }
        }
    }
}
