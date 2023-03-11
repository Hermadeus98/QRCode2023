namespace QRCode.Framework
{
    using System.Threading.Tasks;

    public interface ISaveService : IService
    {
        public GameData GetGameData();
        public Task Initialize();
        public Task NewGame();
        public Task LoadGame();
        public Task SaveGame();
    }
}
