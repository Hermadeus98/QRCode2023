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

            DB.Instance.TryGetDatabase<SceneDatabase>(DBEnum.DB_Scene, out var sceneDatabase);

            foreach (var sceneReference in sceneDatabase.GetDatabase)
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
        
        private static void LoadSceneGroup(SceneDatabase.SceneReferenceGroup sceneReferenceGroup)
        {
            EditorSceneManager.OpenScene(AssetDatabase.GetAssetPath(sceneReferenceGroup.Scenes[0].editorAsset), OpenSceneMode.Single);

            var subScenes = sceneReferenceGroup.Scenes;
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
