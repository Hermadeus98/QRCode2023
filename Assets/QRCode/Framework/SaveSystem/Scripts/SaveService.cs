namespace QRCode.Framework
{
    using System.Threading.Tasks;
    using Debugging;
    using Sirenix.OdinInspector;

    //https://www.google.com/search?client=firefox-b-d&q=save+system+unity#fpstate=ive&vld=cid:47854c46,vid:aUi9aijvpgs,st:899
    public class SaveService : SerializedMonoBehaviour , ISaveService
    {
        private GameData m_gameData = null;
        private IFileDataHandler m_fileDataHandler = null;
        
        public GameData GetGameData()
        {
            return m_gameData;
        }

        public async Task Initialize()
        {
            m_fileDataHandler = FileDataHandlerFactory.CreateFileDataHandler();
            await LoadGame();
        }

        public Task NewGame()
        {
            m_gameData = new GameData();
            QRDebug.Debug(K.DebuggingChannels.SaveSystem, $"New game data was created...");
            return Task.CompletedTask;
        }

        public async Task LoadGame()
        {
            m_gameData = await m_fileDataHandler.Load();
            
            if (m_gameData == null)
            {
                QRDebug.Debug(K.DebuggingChannels.SaveSystem, $"No {nameof(m_gameData)} was found. Initializing default values.");
                await NewGame();
            }
            
            QRDebug.Debug(K.DebuggingChannels.SaveSystem,$"Game is loaded.");
        }

        public async Task SaveGame()
        {
            await m_fileDataHandler.Save(m_gameData);
            
            QRDebug.Debug(K.DebuggingChannels.SaveSystem,$"Game is saved.");
        }

        private async void OnApplicationQuit()
        {
            await SaveGame();
        }
    }
}