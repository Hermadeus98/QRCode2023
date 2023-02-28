namespace QRCode.Framework
{
    #if UNITY_EDITOR
    using UnityEngine;
    using UnityEditor;
    #endif

    public static class PrefabFactory
    {
#if UNITY_EDITOR
        private const string m_inputIconPath = "Assets/QRCode/Framework/UI/InputIcon/Prefabs/P_InputIcon.prefab";
        private const string m_inputIconName= "Input Icon";

        private const int m_UILayerIndex = 5;

        [MenuItem("GameObject/QRCode/UI/Utilities/Input Icon", false, 10)]
        public static void CreateIconPrefab()
        {
            CreatePrefabs(m_inputIconPath, m_inputIconName, true);
        }
        
        private static void CreatePrefabs(string path, string name, bool isUIElement)
        {
            var obj = AssetDatabase.LoadAssetAtPath<InputIcon>(path);

            var selections = Selection.transforms;

            if (selections.Length > 0)
            {
                for (int i = 0; i < selections.Length; i++)
                {
                    CreatePrefab(name, isUIElement, obj, selections[i]);
                }
            }
            else
            {
                CreatePrefab(name, isUIElement, obj, null);
            }
        }

        private static void CreatePrefab(string name, bool isUIElement, InputIcon obj, Transform selection)
        {
            var go = PrefabUtility.InstantiatePrefab(obj, selection) as GameObject;
            go.name = name;

            if (isUIElement)
            {
                go.layer = m_UILayerIndex;
            }
        }
#endif
    }
}
