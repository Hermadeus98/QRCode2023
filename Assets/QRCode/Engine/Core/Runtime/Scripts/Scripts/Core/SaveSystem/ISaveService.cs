namespace QRCode.Engine.Core.SaveSystem
{
    using System;
    using System.Threading.Tasks;
    using Toolbox.Pattern.Service;

    public interface ISaveService : IService
    {
        public GameData GetGameData();
        public Task InitAsync();
        public Task NewGame();
        public Task LoadGameAsync();
        public Task SaveGameAsync();
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
