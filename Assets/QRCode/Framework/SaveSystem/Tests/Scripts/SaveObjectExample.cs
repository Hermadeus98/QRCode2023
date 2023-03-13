namespace QRCode.Framework
{
    using System.Collections.Generic;
    using Sirenix.OdinInspector;
    using UnityEngine;

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
        public async void Save()
        {
            var saveService = ServiceLocator.Current.Get<ISaveService>();
            var gameData = saveService.GetGameData();
            
            SaveData(ref gameData);
            await saveService.SaveGame();
        }
        
        [Button]
        public void Load()
        {
            var saveService = ServiceLocator.Current.Get<ISaveService>();
            var gameData = saveService.GetGameData();
            LoadData(gameData);
        }

        [Button]
        public async void DeleteSave()
        {
            var saveService = ServiceLocator.Current.Get<ISaveService>();
            await saveService.DeleteSave();
        }
        
        public void LoadData(GameData gameData)
        {
            m_savedInt = gameData.ValueTest;
            m_dictionary = new Dictionary<string, float>();
            foreach (var keyValuePair in gameData.DictionaryTest)
            {
                m_dictionary.Add(keyValuePair.Key, keyValuePair.Value);
            }
        }

        public void SaveData(ref GameData gameData)
        {
            gameData.ValueTest = m_savedInt;
            
            gameData.DictionaryTest.Clear();
            gameData.DictionaryTest.Add("key1", 11);
            gameData.DictionaryTest.Add("key2", 141);
            gameData.DictionaryTest.Add("key3", 1414);
        }
    }
}
