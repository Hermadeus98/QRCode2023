namespace QRCode.Framework
{
    using System;
    using System.Threading.Tasks;

    public interface ISaveService : IService
    {
        public GameData GetGameData();
        public Task Initialize();
        public Task NewGame();
        public Task LoadGame();
        public Task SaveGame();
        public Task DeleteSave();
        public bool IsSaving();
        public bool IsLoading();
        public bool IsInit();

        public event Action OnStartSave;
        public event Action OnEndSave;
        public event Action OnStartLoad;
        public event Action OnEndLoad;
    }
}
