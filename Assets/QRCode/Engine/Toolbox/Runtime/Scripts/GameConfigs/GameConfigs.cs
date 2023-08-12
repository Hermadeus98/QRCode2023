namespace QRCode.Engine.Toolbox.GameConfigs
{
    using System.Collections.Generic;
    using System.Linq;
    using Toolbox;
    using Sirenix.OdinInspector;
    using Toolbox.Pattern.Singleton;
#if UNITY_EDITOR
    using UnityEditor;
#endif
    using UnityEngine;

    [CreateAssetMenu(menuName = Constants.GameConfigs.GameConfigsPath)]
    public class GameConfigs : SingletonScriptableObject<GameConfigs>
    {
        [TitleGroup(Constants.InspectorGroups.References)]
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
