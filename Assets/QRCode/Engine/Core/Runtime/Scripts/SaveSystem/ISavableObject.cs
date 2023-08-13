namespace QRCode.Engine.Core.SaveSystem
{
    public interface ISavableObject
    {
        public void SaveGameData(ref GameData gameData);
    }

    public interface ILoadableObject
    {
        public void LoadGameData(GameData gameData);
    }
}