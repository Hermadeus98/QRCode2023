namespace QRCode.Editor.Integration
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Sirenix.OdinInspector;
    using Sirenix.OdinInspector.Editor;
    using UnityEditor;
    using UnityEditor.Localization;
    using UnityEditor.Localization.Plugins.XLIFF.V20;
    using UnityEngine;
    using UnityEngine.Localization;
    using Object = UnityEngine.Object;

    public class CharacterSetCreator : OdinEditorWindow
    {
        [MenuItem("QRCode/Integration/Characters Set Creator")]
        static void Init()
        {
            CharacterSetCreator window = (CharacterSetCreator)EditorWindow.GetWindow(typeof(CharacterSetCreator));

            window.name = "Characters Set Creator Window";
            window.Show();
        }

        [TitleGroup("Settings")] [SerializeField]
        [InfoBox("The analyzed locales.")]
        private LocaleIdentifier[] m_localeIdentifierTargets;

        [FolderPath]
        [SerializeField] private string m_path;

        private Dictionary<LocaleIdentifier, string> m_foundedCharactersPerLocales = null;

        private const string PROGRESS_BAR_TITLE = "Character Set Generation";
        
        [Button]
        private async void Generate()
        {
            EditorUtility.DisplayProgressBar(PROGRESS_BAR_TITLE, "Start Generation.", 0f);
            var allStringTableCollections = FindAssetsByType<StringTableCollection>().ToArray();

            m_foundedCharactersPerLocales = new Dictionary<LocaleIdentifier, string>();

            foreach (var stringTableCollection in allStringTableCollections)
            {
                for (var y = 0; y < m_localeIdentifierTargets.Length; y++)
                {
                    var key = m_localeIdentifierTargets[y];
                    var code = key.Code;
                    
                    EditorUtility.DisplayProgressBar(PROGRESS_BAR_TITLE, $"Start Generation {code}", (float)y / m_localeIdentifierTargets.Length);

                    if (m_foundedCharactersPerLocales.ContainsKey(key) == false)
                    {
                        m_foundedCharactersPerLocales.Add(code, String.Empty);
                    }

                    var characterSet = stringTableCollection.GenerateCharacterSet(key);
                    m_foundedCharactersPerLocales[key] += characterSet;
                    var unique = new HashSet<char>(m_foundedCharactersPerLocales[key]);
                    m_foundedCharactersPerLocales[key] = string.Empty;
                    for (int i = 0; i < unique.Count; i++)
                    {
                        m_foundedCharactersPerLocales[key] += unique.ElementAt(i);
                    }
                }
            }

            await GenerateFiles();
            
            Terminate();
            EditorUtility.DisplayProgressBar(PROGRESS_BAR_TITLE, "Terminate", 1f);
            EditorUtility.ClearProgressBar();
            
            EditorUtility.RequestScriptReload();
        }

        private async Task GenerateFiles()
        {
            foreach (var (code, characterSet) in m_foundedCharactersPerLocales)
            {
                const string extension = ".txt";
                var fileName = $"CharacterSet_{code}{extension}";
                var path = m_path + "/" + fileName;
                await File.WriteAllTextAsync(path, characterSet);
            }
        }
        
        private void Terminate()
        {
            m_foundedCharactersPerLocales.Clear();
            m_foundedCharactersPerLocales = null;
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
