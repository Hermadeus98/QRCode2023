namespace QRCode.Framework
{
    using Sirenix.OdinInspector;
    using UnityEngine;

    public class SaveObjectExample : SerializedMonoBehaviour, IDataPersistence
    {
        [SerializeField] private int m_savedInt;

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
            //await saveService.LoadGame();
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
        }

        public void SaveData(ref GameData gameData)
        {
            gameData.ValueTest = m_savedInt;
        }
    }
}
