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
            DrawSceneSelector();
            
        }

        private static void DrawRightGUI()
        {
            GUILayout.FlexibleSpace();
            PullAllLocalizationTable();
        }

        private static void PullAllLocalizationTable()
        {
            if (GUILayout.Button("Pull LocKit"))
            {
                /*var stringTableCollections = LocalizationEditorSettings.GetStringTableCollections();

                foreach (var collection in stringTableCollections)
                {
                    // Its possible a String Table Collection may have more than one GoogleSheetsExtension.
                    // For example if each Locale we pushed/pulled from a different sheet.
                    foreach (var extension in collection.Extensions)
                    {
                        if (extension is GoogleSheetsExtension googleExtension)
                        {
                            PullExtension(googleExtension);
                        }
                    }
                }*/
            }
        }

        private static void PullExtension(string tableCollectionName)
        {
            /*var tableCollection = LocalizationEditorSettings.GetStringTableCollection(tableCollectionName);
            var googleExtension = tableCollection.Extensions.FirstOrDefault(e => e is GoogleSheetsExtension) as GoogleSheetsExtension;
            if (googleExtension == null)
            {
                Debug.LogError($"String Table Collection {tableCollection.TableCollectionName} Does not contain a Google Sheets Extension.");
                return;
            }

            PullExtension(googleExtension);*/
        }

        /*static void PullExtension(GoogleSheetsExtension googleExtension)
        {
            /#1#/ Setup the connection to Google
            var googleSheets = new GoogleSheets(googleExtension.SheetsServiceProvider);
            googleSheets.SpreadSheetId = googleExtension.SpreadsheetId;

            // Now update the collection. We can pass in an optional ProgressBarReporter so that we can updates in the Editor.
            googleSheets.PullIntoStringTableCollection(googleExtension.SheetId, googleExtension.TargetCollection as StringTableCollection, googleExtension.Columns, reporter: new ProgressBarReporter());#1#
        }*/

        private static void DrawSceneSelector()
        {
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
