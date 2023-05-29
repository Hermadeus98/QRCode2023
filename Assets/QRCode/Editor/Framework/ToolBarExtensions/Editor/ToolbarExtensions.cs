namespace UnityToolbarExtender
{
    using QRCode.Framework;
    using QRCode.Framework.Extensions;
    using QRCode.Framework.SceneManagement;
    using UnityEditor;
    using UnityEditor.SceneManagement;
    using UnityEngine;

    [InitializeOnLoad]
    public static class ToolbarExtensions
    {
        private static GenericMenu m_sceneGenericMenu;
        
        static ToolbarExtensions()
        {
            ToolbarExtender.LeftToolbarGUI.Add(DrawLeftGUI);
            ToolbarExtender.RightToolbarGUI.Add(DrawRightGUI);
        }

        private static void DrawLeftGUI()
        {
            GUILayout.FlexibleSpace();
            m_sceneGenericMenu = new GenericMenu();

            DB.Instance.TryGetDatabase<LevelDatabase>(DBEnum.DB_Levels, out var levelDatabase);

            foreach (var sceneReference in levelDatabase.GetDatabase)
            {
                m_sceneGenericMenu.AddItem(new GUIContent(sceneReference.Key), false, () => LoadSceneGroup(sceneReference.Value));
            }

            if (GUILayout.Button(new GUIContent("Scene Selector")))
            {
                m_sceneGenericMenu.ShowAsContext();
            }
        }

        private static void DrawRightGUI()
        {
            
        }
        
        private static void LoadSceneGroup(LevelReferenceGroup levelReferenceGroup)
        {
            EditorSceneManager.OpenScene(AssetDatabase.GetAssetPath(levelReferenceGroup.Levels[0].editorAsset), OpenSceneMode.Single);

            var subScenes = levelReferenceGroup.Levels;
            if (subScenes.IsNotNullOrEmpty())
            {
                for (int i = 0; i < subScenes.Length; i++)
                {
                    EditorSceneManager.OpenScene(AssetDatabase.GetAssetPath(subScenes[i].editorAsset), OpenSceneMode.Additive);
                }
            }
        }
    }
}
