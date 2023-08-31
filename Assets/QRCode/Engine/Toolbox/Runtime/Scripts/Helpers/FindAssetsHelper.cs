namespace QRCode.Engine.Toolbox.Helpers
{
    using System.Collections.Generic;

#if UNITY_EDITOR
    using UnityEditor;
#endif
    using UnityEngine;

    public static class FindAssetsHelper
    {
#if UNITY_EDITOR
        public static IEnumerable<T> FindAssetsByType<T>() where T : Object {
            var guids = AssetDatabase.FindAssets($"t:{typeof(T)}");
            foreach (var t in guids) {
                var assetPath = AssetDatabase.GUIDToAssetPath(t);
                var asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
                if (asset != null) {
                    yield return asset;
                }
            }
        }
#endif
    }
}
