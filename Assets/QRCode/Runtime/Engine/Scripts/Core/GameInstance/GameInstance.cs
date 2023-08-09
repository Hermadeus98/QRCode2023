namespace QRCode.Engine.Core
{
    using System.Threading.Tasks;
    using Framework.Debugging;
    using UnityEngine;
    using K = Framework.K;

    /// <summary>
    /// <see cref="GameInstance"/> represents the entry point of the game logic.
    /// </summary>
    public sealed class GameInstance
    {
        private static GameInstance m_instance;
        public static GameInstance Instance
        {
            get
            {
                return m_instance;
            }
        }

        private GameInstanceInitializationDataComponent m_gameInstanceInitializationDataComponent = null;
        private GameInstanceEvents m_gameInstanceEvents = null;
        private bool m_managersAreInit = false;

        public bool ManagersAreInit
        {
            get
            {
                return m_managersAreInit;
            }
        }

        public GameInstanceEvents GameInstanceEvents
        {
            get
            {
                return m_gameInstanceEvents;
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetGameInstance()
        {
            m_instance = null;
        }
        
        public GameInstance(GameInstanceInitializationDataComponent gameInstanceInitializationData)
        {
            if (m_instance != null)
            {
                QRDebug.DebugFatal(Constants.EngineConstants.EngineLogChannels.EngineChannel, $"A GameInstance is already existing in current context.");
                return;
            }

            m_instance = this;
            
            m_gameInstanceInitializationDataComponent = gameInstanceInitializationData;
            m_gameInstanceEvents = new GameInstanceEvents();
            
            LoadGame();
        }
        
        private async void LoadGame()
        {
            await InitManagers();
        }

        private async Task InitManagers()
        {
            var managersForGameInstanceInitialization = m_gameInstanceInitializationDataComponent.AllManagersForGameInstanceInitialization;
            var managersLength = managersForGameInstanceInitialization.Length;
            for (var i = 0; i < managersLength; i++)
            {
                await managersForGameInstanceInitialization[i].InitAsync();
            }
            
            QRDebug.DebugInfo(K.DebuggingChannels.Game, $"Managers has been initialize.");
            m_managersAreInit = true;
        }
    }
    
    public struct GameInfo
    {
        public bool GameIsStarted { get; set; }
    }
    
    public struct PauseInfo
    {
        public bool Pause { get; set; }
    }
}
