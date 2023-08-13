namespace QRCode.Engine.Core.GameInstance
{
    using UnityEngine;
    using UnityEngine.AddressableAssets;
    
    using System.Threading.Tasks;
    using System.Threading;

    using Toolbox;
    using Debugging;
    using Managers;
    using SceneManagement;
    using SceneManagement.GeneratedEnum;
    using Toolbox.Optimization;

    /// <summary>
    /// <see cref="GameInstance"/> represents the entry point of the game logic.
    /// </summary>
    public sealed class GameInstance : IDeletable
    {
        #region Fields
        private static GameInstance _instance;
        private GameInstanceInitializationConfig m_gameInstanceInitializationConfig = null;
        private GameInstanceEvents m_gameInstanceEvents = null;
        private IManager[] m_managers = null;
        private bool m_isReady = false;
        private CancellationTokenSource m_cancellationTokenSource;

        private const string MANAGERS_PARENT_NAME = "[MANAGERS]";
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

        public GameInstance(GameInstanceInitializationConfig gameInstanceInitializationConfig)
        {
            if (_instance != null)
            {
                QRDebug.DebugFatal(Engine.Constants.EngineConstants.EngineLogChannels.EngineChannel, $"A GameInstance is already existing in current context.");
                return;
            }
            else
            {
                QRDebug.DebugInfo(Engine.Constants.EngineConstants.EngineLogChannels.EngineChannel, $"GameInstanceCreation.");
            }
            
            _instance = this;
            
            m_gameInstanceInitializationConfig = gameInstanceInitializationConfig;
            m_gameInstanceEvents = new GameInstanceEvents();

            m_cancellationTokenSource = new CancellationTokenSource();

            Application.quitting -= Delete;
            Application.quitting += Delete;
        }

        public void Delete()
        {
            Application.quitting -= Delete;

            if (m_cancellationTokenSource != null)
            {
                m_cancellationTokenSource.Cancel();
                m_cancellationTokenSource.Dispose();
                m_cancellationTokenSource = null;
            }
            
            m_gameInstanceEvents.DeleteAll();
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

        #region Publics
        public async Task LoadGame()
        {
            await InstantiateManagers();
            await InitManagers();
            
            await InstantiateScenes();

            m_isReady = true;
            m_gameInstanceEvents.OnGameInstanceIsReady();
        }
        #endregion
        
        #region Privates

        private async Task InstantiateScenes()
        {
            await SceneManagementService.LoadScene(DB_ScenesEnum.Scene_UI);
        }
        
        private async Task InstantiateManagers()
        {
            var parent = new GameObject(MANAGERS_PARENT_NAME).transform;
            var managersCount = m_gameInstanceInitializationConfig.AllManagersForGameInstanceInitialization.Length;
            m_managers = new IManager[managersCount];
            for (int i = 0; i < managersCount; i++)
            {
                var operation = Addressables.InstantiateAsync(m_gameInstanceInitializationConfig.AllManagersForGameInstanceInitialization[i]);
                var managerGameObject = await operation.Task;
                managerGameObject.transform.SetParent(parent);
                m_managers[i] = managerGameObject.GetComponent<IManager>();
            }
        }
        
        private async Task InitManagers()
        {
            var managersLength = m_managers.Length;
            for (var i = 0; i < managersLength; i++)
            {
                await m_managers[i].InitAsync(m_cancellationTokenSource.Token);
            }
            
            QRDebug.DebugInfo(Constants.DebuggingChannels.Game, $"Managers has been initialize.");
        }

        #endregion
        #endregion
    }
}
