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
        private static SceneDatabase m_sceneDatabase = null;
        private static Dictionary<string, SceneDatabase.SceneReferenceGroup> SceneReferenceGroups = null;

        
        [MenuItem("QRCode/Scene Selector")]
        private static void Init()
        {
            SceneWindowEditor window = (SceneWindowEditor)EditorWindow.GetWindow(typeof(SceneWindowEditor));
            window.titleContent = new GUIContent("Scene Selector");
            window.Show();

            DB.Instance.TryGetDatabase<SceneDatabase>(DBEnum.DB_Scene, out m_sceneDatabase);

            if (m_sceneDatabase == null)
            {
                QRDebug.DebugMessage(LogType.Error, "Editor", $"Impossible to load {nameof(m_sceneDatabase)}.");
                return;
            }
            
            SceneReferenceGroups = new Dictionary<string, SceneDatabase.SceneReferenceGroup>(m_sceneDatabase.GetDatabase);
        }
        
        void OnGUI()
        {
            if (m_sceneDatabase == null)
            {
                return;
            }

            if (SceneReferenceGroups == null)
            {
                SceneReferenceGroups = m_sceneDatabase.GetDatabase;
                return;
            }

            var sceneReferenceGroupLength = SceneReferenceGroups.Count;
            for (int i = 0; i < sceneReferenceGroupLength; i++)
            {
                GUILayout.BeginHorizontal("box");
                string key = SceneReferenceGroups.Keys.ElementAt(i);
                GUILayout.Label($"{key} :");
                if(GUILayout.Button($"LOAD"))
                {
                    var openedScenes = new List<Scene>();
                    
                    for (int j = 0; j < EditorSceneManager.sceneCount; j++)
                    {
                        openedScenes.Add(EditorSceneManager.GetSceneAt(i));
                    }

                    if (openedScenes.Any(scene => scene.isDirty))
                    {
                        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                        {
                            LoadSceneGroup(SceneReferenceGroups[key]);
                        }
                    }
                    else
                    {
                        LoadSceneGroup(SceneReferenceGroups[key]);
                    }
                }
                if(GUILayout.Button($"SELECT", EditorStyles.miniButton))
                {
                    EditorGUIUtility.PingObject(SceneReferenceGroups[key].Scenes[0].editorAsset);
                }
                GUILayout.EndHorizontal();
            }
        }

        private void LoadSceneGroup(SceneDatabase.SceneReferenceGroup sceneReferenceGroup)
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
