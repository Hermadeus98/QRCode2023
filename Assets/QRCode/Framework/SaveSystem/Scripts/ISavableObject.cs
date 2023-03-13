namespace QRCode.Framework
{
    public interface ISavableObject
    {
        public void SaveData(ref GameData gameData);
    }

    public interface ILoadableObject
    {
        public void LoadData(GameData gameData);
    }
}