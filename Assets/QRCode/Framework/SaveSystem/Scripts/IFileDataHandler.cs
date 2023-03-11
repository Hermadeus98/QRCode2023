namespace QRCode.Framework
{
    using System.Threading.Tasks;

    public interface IFileDataHandler
    {
        public Task<GameData> Load();
        public Task Save(GameData gameData);
    }
}