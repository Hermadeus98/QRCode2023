namespace QRCode.Framework
{
    using System.Collections.Generic;
    using System.Linq;
    using Singleton;
    using Sirenix.OdinInspector;
#if UNITY_EDITOR
    using UnityEditor;
#endif
    using UnityEngine;

    [CreateAssetMenu(menuName = K.GameConfigs.GameConfigsPath)]
    public class GameConfigs : SingletonScriptableObject<GameConfigs>
    {
        [TitleGroup(K.InspectorGroups.References)]
        [SerializeField][ReadOnly] private GameConfigBase[] allGameConfigs;

        public T GetCatalogOfType<T>() where T : GameConfigBase
        {
            return allGameConfigs.OfType<T>().FirstOrDefault();
        }
        
        
        [Button]
        private void Fetch()
        {
#if UNITY_EDITOR
            var catalogs = FindAssetsByType<GameConfigBase>();
            allGameConfigs = catalogs as GameConfigBase[] ?? catalogs.ToArray();
#endif
        }
        
        private static IEnumerable<T> FindAssetsByType<T>() where T : Object {
            var guids = AssetDatabase.FindAssets($"t:{typeof(T)}");
            foreach (var t in guids) {
                var assetPath = AssetDatabase.GUIDToAssetPath(t);
                var asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
                if (asset != null) {
                    yield return asset;
                }
            }
        }
    }
}
