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
        private static GameLevelDatabase m_gameLevelDatabase = null;
        private static SceneDatabase m_sceneDatabase = null;
        
        private static Dictionary<string, GameLevelReferenceGroup> LevelReferenceGroups = null;
        private static Dictionary<string, SceneReference> SceneReferenceGroups = null;

        
        [MenuItem("QRCode/Scene Selector")]
        private static void Init()
        {
            var window = (SceneWindowEditor)EditorWindow.GetWindow(typeof(SceneWindowEditor));
            window.titleContent = new GUIContent("Scene Selector");
            window.Show();

            DB.Instance.TryGetDatabase<GameLevelDatabase>(DBEnum.DB_Levels, out m_gameLevelDatabase);
            DB.Instance.TryGetDatabase<SceneDatabase>(DBEnum.DB_Scenes, out m_sceneDatabase);

            if (m_gameLevelDatabase == null)
            {
                QRDebug.DebugMessage(LogType.Error, "Editor", $"Impossible to load {nameof(m_gameLevelDatabase)}.");
                return;
            }
            
            if (m_sceneDatabase == null)
            {
                QRDebug.DebugMessage(LogType.Error, "Editor", $"Impossible to load {nameof(m_sceneDatabase)}.");
                return;
            }
            
            LevelReferenceGroups = new Dictionary<string, GameLevelReferenceGroup>(m_gameLevelDatabase.GetDatabase);
            SceneReferenceGroups = new Dictionary<string, SceneReference>(m_sceneDatabase.GetDatabase);
        }
        
        void OnGUI()
        {
            if (m_gameLevelDatabase == null)
            {
                return;
            }

            if (LevelReferenceGroups == null)
            {
                LevelReferenceGroups = m_gameLevelDatabase.GetDatabase;
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
                    EditorGUIUtility.PingObject(LevelReferenceGroups[key].GameLevel.GameLevelScenes[0].editorAsset);
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

        private void LoadSceneGroup(GameLevelReferenceGroup gameLevelReferenceGroup)
        {
            EditorSceneManager.OpenScene(AssetDatabase.GetAssetPath(gameLevelReferenceGroup.GameLevel.GameLevelScenes[0].editorAsset), OpenSceneMode.Single);

            var subScenes = gameLevelReferenceGroup.GameLevel.GameLevelScenes;
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
