namespace QRCode.Framework
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Game;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using QRCode.Framework.Debugging;

    public class SaveObjectExample : SerializedMonoBehaviour, ILoadableObject, ISavableObject
    {
        [SerializeField] private int m_savedInt;
        [SerializeField] private Dictionary<string, float> m_dictionary;

        private void OnEnable()
        {
            Framework.Save.Current.Register(this);
            Framework.Load.Current.Register(this);
        }

        private void OnDisable()
        {
            Framework.Save.Current.Unregister(this);
            Framework.Load.Current.Unregister(this);
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
                    QRDebug.DebugError(K.DebuggingChannels.SaveManager, $"There is no Save Data.");
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
            m_savedInt = gameData.ValueTest;
            m_dictionary = new Dictionary<string, float>();
            foreach (var keyValuePair in gameData.DictionaryTest)
            {
                m_dictionary.Add(keyValuePair.Key, keyValuePair.Value);
            }
        }

        public void SaveGameData(ref GameData gameData)
        {
            gameData.ValueTest = m_savedInt;
            
            gameData.DictionaryTest.Clear();
            gameData.DictionaryTest.Add("key1", 11);
            gameData.DictionaryTest.Add("key2", 141);
            gameData.DictionaryTest.Add("key3", 1414);
        }
    }
}
