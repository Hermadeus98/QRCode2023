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

        private const string m_progressBarPath = "Assets/QRCode/Framework/UI/Components/ProgressBar/Prefabs/Progress Bar.prefab";
        private const string m_progressBarName = "Progress Bar";

        private const int m_UILayerIndex = 5;

        [MenuItem("GameObject/QRCode/UI/Components/Input Icon", false)]
        public static void CreateInputIconPrefab()
        {
            CreatePrefabs<InputHint>(m_inputIconPath, m_inputIconName, true);
        }

        [MenuItem("GameObject/QRCode/UI/Components/Progress Bar", false)]
        public static void CreateProgressBarPrefab()
        {
            CreatePrefabs<ProgressBar>(m_progressBarPath, m_progressBarName, true);
        }
        
        private static void CreatePrefabs<T>(string path, string name, bool isUIElement) where T : Component
        {
            var obj = AssetDatabase.LoadAssetAtPath<T>(path);

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

        private static void CreatePrefab<T>(string name, bool isUIElement, T obj, Transform selection) where T : Component
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
