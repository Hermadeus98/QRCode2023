namespace UnityToolbarExtender
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using QRCode.Engine.Core.GameLevel;
    using QRCode.Engine.Core.SceneManagement;
    using QRCode.Engine.Toolbox.Database;
    using QRCode.Engine.Toolbox.Database.GeneratedEnums;
    using QRCode.Engine.Toolbox.Extensions;
    using UnityEditor;
    using UnityEditor.Localization;
    using UnityEditor.Localization.Plugins.Google;
    using UnityEditor.SceneManagement;
    using UnityEngine;
    using UnityEngine.Localization;
    using UnityEngine.Localization.Settings;
    using UnityEngine.SceneManagement;
    using Object = UnityEngine.Object;

    [InitializeOnLoad]
    public static class ToolbarExtensions
    {
        private static GenericMenu m_sceneGenericMenu;

        private static bool m_isTestingLocalization;
        
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
            LocalizationTest();
        }

        private static async void LocalizationTest()
        {
            var color = GUI.color;
            if (m_isTestingLocalization)
            {
                GUI.color = new Color(1f, 0.41f, 0.39f);    
            }
            else
            {
                GUI.color = color;
            }
            
            if (GUILayout.Button("Localization Test"))
            {
                if (m_isTestingLocalization)
                {
                    m_isTestingLocalization = false;
                    return;
                }
                
                if (m_isTestingLocalization)
                {
                    return;
                }
                
                m_isTestingLocalization = true;

                List<Locale> allLocales = LocalizationSettings.AvailableLocales.Locales;

                for (int i = 0; i < allLocales.Count(); i++)
                {
                    LocalizationSettings.SelectedLocale = allLocales[i];
                    await Task.Delay(TimeSpan.FromSeconds(0.4f));
                }

                m_isTestingLocalization = false;
            }
        }
        
        private static void PullAllLocalizationTable()
        {
            if (GUILayout.Button("Pull LocKit"))
            {
                var stringTableCollections = UnityEditor.Localization.LocalizationEditorSettings.GetStringTableCollections();

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
                }
            }
        }

        static void PullExtension(GoogleSheetsExtension googleExtension)
        {
            // Setup the connection to Google
            var googleSheets = new GoogleSheets(googleExtension.SheetsServiceProvider);
            googleSheets.SpreadSheetId = googleExtension.SpreadsheetId;

            // Now update the collection. We can pass in an optional ProgressBarReporter so that we can updates in the Editor.
            googleSheets.PullIntoStringTableCollection(googleExtension.SheetId, googleExtension.TargetCollection as StringTableCollection, googleExtension.Columns);
        }

        private static void DrawSceneSelector()
        {
            m_sceneGenericMenu = new GenericMenu();

            DB.Instance.TryGetDatabase<GameLevelDatabase>(DBEnum.DB_Levels, out var levelDatabase);

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
        
        private static void TryLoadSceneGroup(GameLevelReferenceGroup gameLevelReferenceGroup)
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
                    LoadSceneGroup(gameLevelReferenceGroup);
                }
            }
            else
            {
                LoadSceneGroup(gameLevelReferenceGroup);
            }
        }

        private static void LoadSceneGroup(GameLevelReferenceGroup gameLevelReferenceGroup)
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
