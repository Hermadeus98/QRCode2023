namespace QRCode.Editor
{
    using System.Collections.Generic;
    using System.Linq;
    using Framework;
    using Sirenix.OdinInspector;
    using Sirenix.OdinInspector.Editor;
    using Sirenix.Utilities;
    using Sirenix.Utilities.Editor;
    using UnityEditor;
    using UnityEngine;

    public class SettingsEditorWindow : OdinMenuEditorWindow
    {
        [MenuItem("QRCode/Settings")]
        private static void Open()
        {
            var window = GetWindow<SettingsEditorWindow>();
            window.name = "Settings";
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 500);
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree(true);
            tree.DefaultMenuStyle.IconSize = 28f;
            tree.Config.DrawSearchToolbar = true;
            
            var allAsset = FindAssetsByType<SerializedScriptableObject>().OfType<ISetting>();

            tree.AddRange<ISetting>(allAsset, delegate(ISetting o)
            {
                var fullPath = AssetDatabase.GetAssetPath((Object)o);
                const string extension = ".asset";
                var path = fullPath.Substring(0, fullPath.Length - extension.Length);
                var decomposedPath = path.Split('/');
                var sanitizePath = decomposedPath[decomposedPath.Length - 1];
                return sanitizePath;
            });

            return tree;
        }

        public static IEnumerable<T> FindAssetsByType<T>() where T : Object
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