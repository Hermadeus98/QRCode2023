namespace UnityToolbarExtender
{
    using System.Collections.Generic;
    using System.Linq;
    using QRCode.Framework;
    using QRCode.Framework.Extensions;
    using QRCode.Framework.SceneManagement;
    using UnityEditor;
    using UnityEditor.SceneManagement;
    using UnityEngine;
    using UnityEngine.SceneManagement;

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
                m_sceneGenericMenu.AddItem(new GUIContent(sceneReference.Key), false, () => TryLoadSceneGroup(sceneReference.Value));
            }
            
            DB.Instance.TryGetDatabase<SceneDatabase>(DBEnum.DB_Scenes, out var sceneDatabase);

            foreach (var sceneReference in sceneDatabase.GetDatabase)
            {
                m_sceneGenericMenu.AddItem(new GUIContent(sceneReference.Key), false, () => TryLoadScene(sceneReference.Value));
            }

            if (GUILayout.Button(new GUIContent("Scene Selector")))
            {
                m_sceneGenericMenu.ShowAsContext();
            }
        }

        private static void DrawRightGUI()
        {
            
        }

        private static void TryLoadScene(SceneReference sceneReference)
        {
            var openedScenes = new List<Scene>();
                    
            for (int j = 0; j < EditorSceneManager.sceneCount; j++)
            {
                openedScenes.Add(EditorSceneManager.GetSceneAt(j));
            }

            if (openedScenes.Any(scene => scene.isDirty))
            {
                if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                {
                    EditorSceneManager.OpenScene(AssetDatabase.GetAssetPath(sceneReference.Scene.editorAsset), OpenSceneMode.Additive);
                }
            }
            else
            {
                EditorSceneManager.OpenScene(AssetDatabase.GetAssetPath(sceneReference.Scene.editorAsset), OpenSceneMode.Additive);
            }
        }
        
        private static void TryLoadSceneGroup(LevelReferenceGroup levelReferenceGroup)
        {
            var openedScenes = new List<Scene>();
                    
            for (int j = 0; j < EditorSceneManager.sceneCount; j++)
            {
                openedScenes.Add(EditorSceneManager.GetSceneAt(j));
            }

            if (openedScenes.Any(scene => scene.isDirty))
            {
                if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                {
                    LoadSceneGroup(levelReferenceGroup);
                }
            }
            else
            {
                LoadSceneGroup(levelReferenceGroup);
            }
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

        private static void LoadScene(SceneReference sceneReference)
        {
            
        }
    }
}
