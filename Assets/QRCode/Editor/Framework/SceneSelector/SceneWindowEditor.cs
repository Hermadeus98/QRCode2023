namespace QRCode.Editor.SceneSelector
{
    using System.Collections.Generic;
    using System.Linq;
    using Framework;
    using QRCode.Framework.Debugging;
    using QRCode.Framework.Extensions;
    using QRCode.Framework.SceneManagement;
    using UnityEditor;
    using UnityEditor.SceneManagement;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using LogType = QRCode.Framework.Debugging.LogType;

    public class SceneWindowEditor : EditorWindow
    {
        private static LevelDatabase m_levelDatabase = null;
        private static SceneDatabase m_sceneDatabase = null;
        
        private static Dictionary<string, LevelReferenceGroup> LevelReferenceGroups = null;
        private static Dictionary<string, SceneReference> SceneReferenceGroups = null;

        
        [MenuItem("QRCode/Scene Selector")]
        private static void Init()
        {
            var window = (SceneWindowEditor)EditorWindow.GetWindow(typeof(SceneWindowEditor));
            window.titleContent = new GUIContent("Scene Selector");
            window.Show();

            DB.Instance.TryGetDatabase<LevelDatabase>(DBEnum.DB_Levels, out m_levelDatabase);
            DB.Instance.TryGetDatabase<SceneDatabase>(DBEnum.DB_Scenes, out m_sceneDatabase);

            if (m_levelDatabase == null)
            {
                QRDebug.DebugMessage(LogType.Error, "Editor", $"Impossible to load {nameof(m_levelDatabase)}.");
                return;
            }
            
            if (m_sceneDatabase == null)
            {
                QRDebug.DebugMessage(LogType.Error, "Editor", $"Impossible to load {nameof(m_sceneDatabase)}.");
                return;
            }
            
            LevelReferenceGroups = new Dictionary<string, LevelReferenceGroup>(m_levelDatabase.GetDatabase);
            SceneReferenceGroups = new Dictionary<string, SceneReference>(m_sceneDatabase.GetDatabase);
        }
        
        void OnGUI()
        {
            if (m_levelDatabase == null)
            {
                return;
            }

            if (LevelReferenceGroups == null)
            {
                LevelReferenceGroups = m_levelDatabase.GetDatabase;
                return;
            }

            var levelReferenceGroupsCount = LevelReferenceGroups.Count;
            for (var i = 0; i < levelReferenceGroupsCount; i++)
            {
                GUILayout.BeginHorizontal("box");
                var key = LevelReferenceGroups.Keys.ElementAt(i);
                GUILayout.Label($"{key} :");
                if(GUILayout.Button($"LOAD"))
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
                            LoadSceneGroup(LevelReferenceGroups[key]);
                        }
                    }
                    else
                    {
                        LoadSceneGroup(LevelReferenceGroups[key]);
                    }
                }
                if(GUILayout.Button($"SELECT", EditorStyles.miniButton))
                {
                    EditorGUIUtility.PingObject(LevelReferenceGroups[key].Levels[0].editorAsset);
                }
                GUILayout.EndHorizontal();
            }

            var sceneReferenceCount = SceneReferenceGroups.Count;
            for (var i = 0; i < sceneReferenceCount; i++)
            {
                GUILayout.BeginHorizontal("box");
                var key = SceneReferenceGroups.Keys.ElementAt(i);
                GUILayout.Label($"{key} :");
                if(GUILayout.Button($"LOAD"))
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
                            EditorSceneManager.OpenScene(AssetDatabase.GetAssetPath(SceneReferenceGroups[key].Scene.editorAsset), OpenSceneMode.Additive);
                        }
                    }
                    else
                    {
                        EditorSceneManager.OpenScene(AssetDatabase.GetAssetPath(SceneReferenceGroups[key].Scene.editorAsset), OpenSceneMode.Additive);
                    }
                }
                if(GUILayout.Button($"SELECT", EditorStyles.miniButton))
                {
                    EditorGUIUtility.PingObject(SceneReferenceGroups[key].Scene.editorAsset);
                }
                GUILayout.EndHorizontal();
            }
        }

        private void LoadSceneGroup(LevelReferenceGroup levelReferenceGroup)
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
