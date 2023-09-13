namespace QRCode.Engine.Core.SaveSystem.Tests
{
    using System.Collections.Generic;
    using QRCode.Engine.Core.Tags;
    using QRCode.Engine.Debugging;
    using Sirenix.OdinInspector;
    using UnityEngine;


    public class SaveObjectExample : SerializedMonoBehaviour, ILoadableObject, ISavableObject
    {
        [SerializeField] private int _savedInt;
        [SerializeField] private Dictionary<string, float> _dictionary;

        private void OnEnable()
        {
            SaveSystem.Save.Current.Register(this);
            SaveSystem.Load.Current.Register(this);
        }

        private void OnDisable()
        {
            SaveSystem.Save.Current.Unregister(this);
            SaveSystem.Load.Current.Unregister(this);
        }

        [Button]
#pragma warning disable CS1998
        private async void Save()
#pragma warning restore CS1998
        {
#if UNITY_EDITOR
            SaveManager.SaveInEditor();          
#else
            var saveService = SaveManager.Instance;
            var gameData = saveService.GetGameData();
            
            SaveGameData(ref gameData);
            await saveService.SaveGameAsync();
#endif
        }
        
        [Button]
        private async void Load()
        {
#if UNITY_EDITOR
            if (Application.isPlaying == false)
            {
                var gameDataInEditor = await SaveManager.LoadInEditor();

                if (gameDataInEditor == null)
                {
                    QRLogger.DebugError<CoreTags.Save>($"There is no Save Data.", gameObject);
                    return;
                }

                LoadGameData(gameDataInEditor);
                return;
            }
#endif
            var saveService = SaveManager.Instance;
            var gameData = saveService.GetGameData();
            LoadGameData(gameData);
        }

        [Button]
        private async void DeleteSave()
        {
            var saveService = SaveManager.Instance;
            await saveService.DeleteSave();
        }
        
        public void LoadGameData(GameData gameData)
        {
            _savedInt = gameData.ValueTest;
            _dictionary = new Dictionary<string, float>();
            foreach (var keyValuePair in gameData.DictionaryTest)
            {
                _dictionary.Add(keyValuePair.Key, keyValuePair.Value);
            }
        }

        public void SaveGameData(ref GameData gameData)
        {
            gameData.ValueTest = _savedInt;
            
            gameData.DictionaryTest.Clear();
            gameData.DictionaryTest.Add("key1", 11);
            gameData.DictionaryTest.Add("key2", 141);
            gameData.DictionaryTest.Add("key3", 1414);
        }
    }
}
