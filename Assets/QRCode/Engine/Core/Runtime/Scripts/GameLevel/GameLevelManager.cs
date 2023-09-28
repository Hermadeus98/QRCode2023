namespace QRCode.Engine.Core.GameLevels
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using QRCode.Engine.Core.GameLevels.GeneratedEnums;
    using QRCode.Engine.Core.Manager;
    using QRCode.Engine.Core.SaveSystem;
    using QRCode.Engine.Core.Tags;
    using QRCode.Engine.Core.UI.LoadingScreen;
    using QRCode.Engine.Core.UI.LoadingScreen.GeneratedEnums;
    using QRCode.Engine.Debugging;
    using QRCode.Engine.Toolbox.Database;
    using QRCode.Engine.Toolbox.Database.GeneratedEnums;
    using QRCode.Engine.Toolbox.Extensions;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UnityEngine.AddressableAssets;
    using UnityEngine.ResourceManagement.AsyncOperations;
    using UnityEngine.ResourceManagement.ResourceProviders;
    using UnityEngine.SceneManagement;
    using Constants = QRCode.Engine.Toolbox.Constants;
    
    /// <summary>
    /// The <see cref="GameLevelManager"/> will manage the loading/unloading and initialization of game levels.
    /// </summary>
    public class GameLevelManager : GenericManagerBase<GameLevelManager>
    {
        #region Internals
        /// <summary>
        /// This struct stocks all in information about <see cref="AGameLevel"/> loading.
        /// </summary>
        [Serializable]
        public struct GameLevelLoadingInfo
        {
            [ReadOnly][ProgressBar(0f, 1f)] public float GlobalProgress;
            [ReadOnly] public string ProgressDescription;
            [ReadOnly] public GameLevelLoadingStatus GameLevelLoadingStatus;

            /// <summary>
            /// The default values for <see cref="GameLevelLoadingInfo"/>.
            /// </summary>
            public static GameLevelLoadingInfo Default
            {
                get
                {
                    return new GameLevelLoadingInfo()
                    {
                        GlobalProgress = 0.0f,
                        ProgressDescription = string.Empty,
                        GameLevelLoadingStatus = GameLevelLoadingStatus.NotLoaded,
                    };
                }
            }
        }

        /// <summary>
        /// The status about a loading of a <see cref="AGameLevel"/>.
        /// </summary>
        public enum GameLevelLoadingStatus
        {
            NotLoaded = 0, //
            SceneAreLoading = 1,
            SceneAreLoaded = 2,
            InitializationIsLoading = 3,
            InitializationIsDone = 4,
        }
        #endregion Internals

        #region Fields
        #region Serialized
        [TitleGroup(Constants.InspectorGroups.Debugging)]
        [Tooltip("The current game level loading information.")]
        [SerializeField] private GameLevelLoadingInfo _gameLevelLoadingInfo = GameLevelLoadingInfo.Default;
        #endregion Serialized

        private SaveManager _saveManager = null;
        private LoadingScreenManager _loadingScreenManager = null;
        
        private GameLevelDatabase _gameLevelDatabase = null;
        private GameLevelManagerSettings _gameLevelManagerSettings = null;
        private AGameLevelData _currentLevelLoaded = null;
        
        private List<AsyncOperationHandle<SceneInstance>> _sceneInstanceHandles = null;
        private bool _isLoading = false;
        #endregion Fields

        #region Properties
        /// <summary>
        /// Return true if a <see cref="AGameLevel"/> is loading.
        /// </summary>
        public bool IsLoading => _isLoading;
        #endregion Properties

        #region Events
        private event Func<Task> _startToLoadAsync = null;
        private event Action _startToLoad = null;
        private event Action<GameLevelLoadingInfo> _loading = null;
        private event Action _finishToLoad = null;
        private event Func<Task> _finishToLoadAsync = null;

        /// <summary>
        /// Func call when a <see cref="AGameLevel"/> start to load, can add async callbacks.
        /// </summary>
        public event Func<Task> StartToLoadLevelAsync
        {
            add
            {
                _startToLoadAsync -= value;
                _startToLoadAsync += value;
            }
            remove
            {
                _startToLoadAsync -= value;
            }
        }
        
        /// <summary>
        /// Event when a <see cref="AGameLevel"/> start to load.
        /// </summary>
        public event Action StartToLoadLevel
        {
            add
            {
                _startToLoad -= value;
                _startToLoad += value;
            }
            remove
            {
                _startToLoad -= value;
            }
        }
        
        /// <summary>
        /// Event while a <see cref="AGameLevel"/> is loading.
        /// </summary>
        public event Action<GameLevelLoadingInfo> LoadingLevel
        {
            add
            {
                _loading -= value;
                _loading += value;
            }
            remove
            {
                _loading -= value;
            }
        }
        
        /// <summary>
        /// Event when a <see cref="AGameLevel"/> start to unload.
        /// </summary>
        public event Action FinishToLoadLevel
        {
            add
            {
                _finishToLoad -= value;
                _finishToLoad += value;
            }
            remove
            {
                _finishToLoad -= value;
            }
        }
        
        /// <summary>
        /// Func call when a <see cref="AGameLevel"/> start to unload, can add async callbacks.
        /// </summary>
        public event Func<Task> FinishToLoadLevelAsync
        {
            add
            {
                _finishToLoadAsync -= value;
                _finishToLoadAsync += value;
            }
            remove
            {
                _finishToLoadAsync -= value;
            }
        }
        #endregion Events

        #region Methods
        #region Lifecycle
        protected override Task InitAsync(CancellationToken cancellationToken)
        {
            _saveManager = SaveManager.Instance;
            _gameLevelManagerSettings = GameLevelManagerSettings.Instance;
            _loadingScreenManager = LoadingScreenManager.Instance;
            _gameLevelDatabase = DB.Instance.GetDatabase<GameLevelDatabase>(DBEnum.DB_GameLevels);
            _sceneInstanceHandles = new List<AsyncOperationHandle<SceneInstance>>();
            
            return Task.CompletedTask;
        }
        
        public override void Delete()
        {
            if (_sceneInstanceHandles != null)
            {
                var handlesCount = _sceneInstanceHandles.Count;

                if (handlesCount <= 0)
                {
                    return;
                }

                for (var i = 0; i < handlesCount; i++)
                {
                    if (_sceneInstanceHandles[i].IsValid())
                    {
                        Addressables.ReleaseInstance(_sceneInstanceHandles[i]);
                    }
                }

                _sceneInstanceHandles.Clear();
            }

            _saveManager = null;
            _gameLevelDatabase = null;
            _currentLevelLoaded = null;
            _gameLevelManagerSettings = null;
            
            _startToLoadAsync = null;
            _startToLoad = null;
            _loading = null;
            _finishToLoad = null;
            _finishToLoadAsync = null;
            
            base.Delete();
        }
        #endregion Lifecycle
        
        #region Public Methods
        /// <summary>
        /// Change the current <see cref="AGameLevel"/> in the game, will unload the current loaded <see cref="AGameLevel"/>.
        /// </summary>
        public async Task ChangeLevel(DB_GameLevelsEnum gameLevelToLoad, DB_LoadingScreenEnum loadingScreenEnum, bool forceReload = false, bool activateOnLoad = true, int priority = 100)
        {
            if (_isLoading)
            {
                QRLogger.DebugError<CoreTags.GameLevels>("A scene is already in loading...");
                return;
            }

            _gameLevelLoadingInfo = new GameLevelLoadingInfo()
            {
                GlobalProgress = 0f,
                ProgressDescription = _gameLevelManagerSettings.LoadingLocalizedString,
            };
            
            _isLoading = true;
            
            if (_gameLevelDatabase.TryGetInDatabase(gameLevelToLoad.ToString(), out var levelReferenceGroup))
            {
                // If a level is already loaded, it's managed differently.
                if (forceReload == false && _currentLevelLoaded != null)
                {
                    if (levelReferenceGroup.name == _currentLevelLoaded.name)
                    {
                        QRLogger.DebugInfo<CoreTags.GameLevels>($"{gameLevelToLoad.ToString()} is already loaded.", _gameLevelDatabase);
                        await ManageLoadedGameLevel(loadingScreenEnum);
                        _isLoading = false;
                        return;
                    }
                }

                LoadingScreenHandle loadingScreenHandle = await _loadingScreenManager.GetLoadingScreen(loadingScreenEnum);
                ILoadingScreen loadingScreen = loadingScreenHandle.LoadingScreen;
                loadingScreen.Progress(_gameLevelLoadingInfo.GlobalProgress, _gameLevelLoadingInfo.ProgressDescription);
                await loadingScreen.Show();

                if (_currentLevelLoaded != null)
                {
                    await UnloadLevelInternal(_currentLevelLoaded);
                }

                await LoadGameLevelInternal(gameLevelToLoad, loadingScreen, forceReload, activateOnLoad, priority);
                await _loadingScreenManager.HideLoadingScreen(loadingScreenHandle);
            }

            _isLoading = false;
        }

        /// <summary>
        /// Unload the current <see cref="AGameLevel"/>.
        /// </summary>
        public async Task UnloadCurrentLevel()
        {
            if (_currentLevelLoaded != null)
            {
                await UnloadLevelInternal(_currentLevelLoaded);
            }
        }
        #endregion Public Methods

        #region Private Methods
        /// <summary>
        /// Load a <see cref="AGameLevel"/> from the <see cref="DB_GameLevelsEnum"/>.
        /// </summary>
        private async Task LoadGameLevelInternal(DB_GameLevelsEnum gameLevelToLoad, ILoadingScreen loadingScreen, bool forceReload = false, bool activateOnLoad = true, int priority = 100)
        {
            if (_gameLevelDatabase.TryGetInDatabase(gameLevelToLoad.ToString(), out var levelReferenceGroup))
            {
                if (_startToLoadAsync != null)
                {
                    await _startToLoadAsync.Invoke();
                }

                _startToLoad?.Invoke();
                _gameLevelLoadingInfo.GameLevelLoadingStatus = GameLevelLoadingStatus.NotLoaded;

                if (forceReload)
                {
                    await UnloadGameLevelInternal(gameLevelToLoad);
                }

                LoadingLevel += delegate(GameLevelLoadingInfo info)
                {
                    loadingScreen.Progress(info.GlobalProgress, info.ProgressDescription);
                };
                
                await LoadGameLevelInternal(levelReferenceGroup, activateOnLoad, priority);
            }
            else
            {
                QRLogger.DebugError<CoreTags.GameLevels>($"Cannot load {gameLevelToLoad.ToString()}, verify SceneDatabase.", _gameLevelDatabase);
            }
        }

        /// <summary>
        /// Unload a <see cref="AGameLevel"/> from the <see cref="DB_GameLevelsEnum"/>.
        /// </summary>
        private async Task UnloadGameLevelInternal(DB_GameLevelsEnum gameLevelToUnload)
        {
            if (_gameLevelDatabase.TryGetInDatabase(gameLevelToUnload.ToString(), out var foundedObject))
            {
                await UnloadLevelInternal(foundedObject);
            }
        }
        
        /// <summary>
        /// If a <see cref="AGameLevel"/> is already loaded, this function must be call.
        /// </summary>
        private async Task ManageLoadedGameLevel(DB_LoadingScreenEnum loadingScreenEnum)
        {
            _isLoading = true;

            LoadingScreenHandle loadingScreenHandle = await _loadingScreenManager.GetLoadingScreen(loadingScreenEnum);

            void UpdateLoadingScreen(GameLevelLoadingInfo info)
            {
                loadingScreenHandle.LoadingScreen.Progress(info.GlobalProgress, info.ProgressDescription);
            }
            
            UpdateLoadingScreen(_gameLevelLoadingInfo);
            LoadingLevel += UpdateLoadingScreen;
            
            await loadingScreenHandle.LoadingScreen.Show();

            UpdateLoadingGameLevel();

            await InitializeLoadedLevel();
            
            if (SaveServiceSettings.Instance.LoadAsyncAfterSceneLoading)
            {
                Load.Current.LoadObjects();
            }

            LoadingLevel -= UpdateLoadingScreen;
            
            var hideTask = _loadingScreenManager.HideLoadingScreen(loadingScreenHandle);
            await hideTask;

            _gameLevelLoadingInfo.GlobalProgress = 1f;
            _gameLevelLoadingInfo.GameLevelLoadingStatus = GameLevelLoadingStatus.SceneAreLoaded;
            _isLoading = false;
        }
        
        private async Task LoadGameLevelInternal(AGameLevelData aGameLevelReferenceGroupToLoad, bool activateOnLoad = true, int priority = 100)
        {
            if (_currentLevelLoaded != null)
            {
                if (_currentLevelLoaded.GetHashCode() == aGameLevelReferenceGroupToLoad.GetHashCode())
                {
                    QRLogger.DebugError<CoreTags.GameLevels>($"{nameof(_currentLevelLoaded)} already contain {aGameLevelReferenceGroupToLoad.ToString()}.");
                    return;
                }
            }

            _gameLevelLoadingInfo.GameLevelLoadingStatus = GameLevelLoadingStatus.SceneAreLoading;

            UpdateLoadingGameLevel();
            
            if (aGameLevelReferenceGroupToLoad.GameLevelScenes.IsNotNullOrEmpty())
            {
                var sceneReferenceGroupToLoadCount = aGameLevelReferenceGroupToLoad.GameLevelScenes.Length;
                for (var i = 0; i < aGameLevelReferenceGroupToLoad.GameLevelScenes.Length; i++)
                {
                    void OnLoadingSubScene(AsyncOperationHandle<SceneInstance> operation)
                    {
                        var currentSceneLoadingProgress = ((i + operation.GetDownloadStatus().Percent) / aGameLevelReferenceGroupToLoad.GameLevelScenes.Length) /2f;
                        _gameLevelLoadingInfo.GlobalProgress = currentSceneLoadingProgress;
                    }
                    
                    void OnEndLoadingSubScene(AsyncOperationHandle<SceneInstance> operation)
                    {
                        _gameLevelLoadingInfo.GlobalProgress = ((i + 1f) / sceneReferenceGroupToLoadCount) /2f;
                    }

                    await LoadScene(aGameLevelReferenceGroupToLoad.GameLevelScenes[i], OnLoadingSubScene, OnEndLoadingSubScene, activateOnLoad, priority);
                }
            }

            _currentLevelLoaded = aGameLevelReferenceGroupToLoad;
            QRLogger.DebugInfo<CoreTags.GameLevels>($"{aGameLevelReferenceGroupToLoad.name} is loaded.");

            await InitializeLoadedLevel();
            
            if (SaveServiceSettings.Instance.LoadAsyncAfterSceneLoading)
            {
                if (_saveManager == null)
                {
                    return;
                }
                
                await _saveManager.LoadGameAsync();
                Load.Current.LoadObjects();
            }
            
            _finishToLoad?.Invoke();
            if (_finishToLoadAsync != null)
            {
                await _finishToLoadAsync.Invoke();
            }
            
            _gameLevelLoadingInfo.GameLevelLoadingStatus = GameLevelLoadingStatus.SceneAreLoaded;
        }

        /// <summary>
        /// This function is executed while a <see cref="AGameLevel"/> is loading.
        /// </summary>
        private async void UpdateLoadingGameLevel()
        {
            while (_isLoading)
            {
                if (CancellationTokenSource.IsCancellationRequested)
                {
                    return;
                }
                
                _loading?.Invoke(_gameLevelLoadingInfo);
                await Task.Yield();
            }
        }
        
        /// <summary>
        /// Unload a <see cref="AGameLevel"/>.
        /// </summary>
        private async Task UnloadLevelInternal(AGameLevelData aGameLevelReferenceGroupToUnload)
        {
            if (_currentLevelLoaded != null)
            {
                if (SaveServiceSettings.Instance.SaveAsyncBeforeSceneLoading)
                {
                    Save.Current.SaveObjects();
                    await _saveManager.SaveGameAsync();
                }
                
                var levelInitialization = AGameLevel.Current;
                if (levelInitialization != null)
                {
                    levelInitialization.UnloadLevel();
                }
                
                foreach (var sceneReference in aGameLevelReferenceGroupToUnload.GameLevelScenes)
                {
                    try
                    {
                        // If the scene was loaded with addressable, it must be unloaded here.
                        var unloadSceneOperation = sceneReference.UnLoadScene();

                        while (!unloadSceneOperation.IsDone && !CancellationTokenSource.IsCancellationRequested)
                        {
                            await Task.Yield();
                        }
                    }
                    catch (Exception e)
                    {
#if UNITY_EDITOR
                        // Cannot unload an addressable scene already loaded, so the scene is unload with its editor name.
                        var op = SceneManager.UnloadSceneAsync(sceneReference.editorAsset.name);
#else
                        Console.WriteLine(e);
                        throw;
#endif
                    }
                }

                _currentLevelLoaded = null;
                QRLogger.DebugInfo<CoreTags.GameLevels>($"{aGameLevelReferenceGroupToUnload.name} is unloaded.");   
            }
            else
            {
                QRLogger.DebugInfo<CoreTags.GameLevels>($"{aGameLevelReferenceGroupToUnload.name} is already unloaded.");
            }
        }

        /// <summary>
        /// Load a scene of a <see cref="AGameLevel"/>.
        /// </summary>
        private async Task LoadScene(AssetReference sceneObject, Action<AsyncOperationHandle<SceneInstance>> onLoading, Action<AsyncOperationHandle<SceneInstance>> onEndLoading, bool activateOnLoad = true, int priority = 100)
        {
            var loadingSceneObject = sceneObject.LoadSceneAsync(LoadSceneMode.Additive, activateOnLoad, priority);

            if (loadingSceneObject.IsValid())
            {
                _sceneInstanceHandles.Add(loadingSceneObject);
            }

            _gameLevelLoadingInfo.GameLevelLoadingStatus = GameLevelLoadingStatus.SceneAreLoading;
            while(!loadingSceneObject.IsDone && !CancellationTokenSource.IsCancellationRequested)
            {
                onLoading?.Invoke(loadingSceneObject);
                await Task.Yield();
            }
            
            onEndLoading?.Invoke(loadingSceneObject);
        }

        /// <summary>
        /// Initialize the current <see cref="AGameLevel"/>.
        /// </summary>
        private async Task InitializeLoadedLevel()
        {
            var currentGameLevel = AGameLevel.Current;

            if (currentGameLevel == null)
            {
                return;
            }
            
            _gameLevelLoadingInfo.GameLevelLoadingStatus = GameLevelLoadingStatus.InitializationIsLoading;

            var progression = new Progress<GameLevels.GameLevelLoadingInfo>(value =>
            {
                _gameLevelLoadingInfo.GlobalProgress = .5f + (value.LoadingProgressPercent / 2f);
            });

            var loading = currentGameLevel.LoadLevel(CancellationTokenSource.Token, progression);
            await loading;

            _gameLevelLoadingInfo.GlobalProgress = 1f;

            _gameLevelLoadingInfo.GameLevelLoadingStatus = GameLevelLoadingStatus.InitializationIsDone;
        }
        #endregion Private Methods

        #region Editor
#if UNITY_EDITOR
        /// <summary>
        /// This function is used when a game level is already loaded.
        /// </summary>
        public void EditorSetAsAlreadyLoadedLevel(AGameLevelData aGameLevelReferenceGroup)
        {
            _currentLevelLoaded = aGameLevelReferenceGroup;
        }
#endif
        #endregion Editor
        #endregion Methods
    }
}
