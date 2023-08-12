namespace QRCode.Toolbox.Editor.Database
{
    using System.Collections.Generic;
    using System.Linq;
    using Engine.Toolbox.Database;
    using Sirenix.OdinInspector.Editor;
    using Sirenix.Utilities;
    using Sirenix.Utilities.Editor;
    using UnityEditor;
    using UnityEngine;

    public class DatabaseEditorWindow : OdinMenuEditorWindow
    {
        [MenuItem("QRCode/DB")]
        private static void Open()
        {
            var window = GetWindow<DatabaseEditorWindow>();
            window.name = "Databases";
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 500);
        }
        
        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree(true);
            tree.DefaultMenuStyle.IconSize = 28f;
            tree.Config.DrawSearchToolbar = true;

            var allAsset = FindAssetsByType<ScriptableObject>().OfType<IDatabase>();

            tree.AddRange<IDatabase>(allAsset, delegate(IDatabase o)
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
        
        private void AddDragHandles(OdinMenuItem menuItem)
        {
            menuItem.OnDrawItem += x => DragAndDropUtilities.DragZone(menuItem.Rect, menuItem.Value, false, false);
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
