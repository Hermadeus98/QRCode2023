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
        #region Fields
        private static GameInstance _instance;
        private GameInstanceInitializationDataComponent m_gameInstanceInitializationDataComponent = null;
        private GameInstanceEvents m_gameInstanceEvents = null;
        private bool m_isReady = false;
        #endregion

        #region Properties
        public static GameInstance Instance
        {
            get
            {
                return _instance;
            }
        }

        public bool IsReady
        {
            get
            {
                return m_isReady;
            }
        }

        public GameInstanceEvents GameInstanceEvents
        {
            get
            {
                return m_gameInstanceEvents;
            }
        }
        #endregion

        #region Constructors

        public GameInstance(GameInstanceInitializationDataComponent gameInstanceInitializationData)
        {
            if (_instance != null)
            {
                QRDebug.DebugFatal(Constants.EngineConstants.EngineLogChannels.EngineChannel, $"A GameInstance is already existing in current context.");
                return;
            }

            _instance = this;
            
            m_gameInstanceInitializationDataComponent = gameInstanceInitializationData;
            m_gameInstanceEvents = new GameInstanceEvents();
            
            LoadGame();
        }

        #endregion

        #region Methods

        #region Lifecycle

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetGameInstance()
        {
            _instance = null;
        }

        #endregion

        #region Privates

        private async void LoadGame()
        {
            await InitManagers();
            m_isReady = true;
        }

        private async Task InitManagers()
        {
            await Task.Delay(10);
            var managersForGameInstanceInitialization = m_gameInstanceInitializationDataComponent.AllManagersForGameInstanceInitialization;
            var managersLength = managersForGameInstanceInitialization.Length;
            for (var i = 0; i < managersLength; i++)
            {
                await managersForGameInstanceInitialization[i].InitAsync();
            }
            
            QRDebug.DebugInfo(K.DebuggingChannels.Game, $"Managers has been initialize.");
        }

        #endregion

        #endregion
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
