namespace QRCode.Engine.Core.GameInstance
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using QRCode.Engine.Core.SceneManagement;
    using QRCode.Engine.Core.SceneManagement.GeneratedEnum;
    using QRCode.Engine.Core.Tags;
    using QRCode.Engine.Debugging;
    using QRCode.Engine.Toolbox.Optimization;
    using UnityEngine;

    /// <summary>
    /// <see cref="GameInstance"/> represents the entry point of the game logic.
    /// </summary>
    public sealed class GameInstance : IDeletable
    {
        #region Fields
        private static GameInstance _instance = null;
        private GameInstanceInitializationConfig _gameInstanceInitializationConfig = null;
        private GameInstanceEvents _gameInstanceEvents = null;
        private bool _isReady = false;
        private CancellationTokenSource _cancellationTokenSource = null;
        #endregion

        #region Properties
        public static GameInstance Instance { get { return _instance; } }

        public bool IsReady { get { return _isReady; } }

        public GameInstanceEvents GameInstanceEvents { get { return _gameInstanceEvents; } }
        #endregion

        #region Constructors

        public GameInstance(GameInstanceInitializationConfig gameInstanceInitializationConfig)
        {
            if (_instance != null)
            {
                QRLogger.DebugFatal<CoreTags.GameInstance>($"A GameInstance is already existing in current context.");
                return;
            }
            
            QRLogger.DebugInfo<CoreTags.GameInstance>($"GameInstanceCreation.");
            
            _instance = this;
            
            _gameInstanceInitializationConfig = gameInstanceInitializationConfig;
            _gameInstanceEvents = new GameInstanceEvents();

            _cancellationTokenSource = new CancellationTokenSource();

            Application.quitting -= Delete;
            Application.quitting += Delete;
        }

        public void Delete()
        {
            Application.quitting -= Delete;

            _instance = null;
            _gameInstanceInitializationConfig = null;

            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource.Dispose();
                _cancellationTokenSource = null;
            }

            if (_gameInstanceEvents != null)
            {
                _gameInstanceEvents.Delete();
                _gameInstanceEvents = null;
            }
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
            await InstantiateScenes();

            _isReady = true;
            _gameInstanceEvents.OnGameInstanceIsReady();
        }
        #endregion
        
        #region Privates

        private async Task InstantiateScenes()
        {
            await SceneManager.Instance.WaitManagerInitialization(_cancellationTokenSource.Token);
            await SceneManager.Instance.LoadScene(DB_ScenesEnum.Scene_UI, new Progress<SceneLoadingInfo>());
        }
        #endregion
        #endregion
    }
}
