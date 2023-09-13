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
    using QRCode.Engine.Toolbox.Optimization;
    using Constants = QRCode.Engine.Toolbox.Constants;
    using UI = QRCode.Engine.Core.UI.UI;
    
    using Sirenix.OdinInspector;
    
    using UnityEngine;
    using UnityEngine.AddressableAssets;
    using UnityEngine.ResourceManagement.AsyncOperations;
    using UnityEngine.ResourceManagement.ResourceProviders;
    using UnityEngine.SceneManagement;
    
    //TODO QR - Simplifier cette class. Clean
    
    /// <summary>
    /// The <see cref="GameLevelManager"/> will manage the loading/unloading and initialization of game levels.
    /// </summary>
    public class GameLevelManager : GenericManagerBase<GameLevelManager>, IDeletable
    {
        #region FIELDS
        #region Serialized
        [TitleGroup(Constants.InspectorGroups.Debugging)][ReadOnly]
        [SerializeField] private SceneLoadingInfo m_sceneLoadingInfo;
        #endregion Serialized

        #region Privates
        private SaveManager m_saveManager = null;
        
        private GameLevelDatabase m_gameLevelDatabase = null;
        private GameLevelManagerSettings m_gameLevelManagerSettings = null;
        private GameLevelData m_levelLoaded = null;
        private LoadingScreenManager m_loadingScreenManager = null;
        
        private List<AsyncOperationHandle<SceneInstance>> m_sceneInstanceHandles = null;
        
        private bool m_isLoading = false;
        #endregion Privates
        #endregion FIELDS

        #region Properties
        public bool IsLoading => m_isLoading;
        #endregion Properties

        #region EVENTS
        private event Func<Task> m_startToLoadAsync = null;
        private event Action m_startToLoad = null;
        private event Action<SceneLoadingInfo> m_loading = null;
        private event Action m_finishToLoad = null;
        private event Func<Task> m_finishToLoadAsync = null;

        /// <summary>
        /// Func call when a <see cref="GameLevel"/> start to load, can add async callbacks.
        /// </summary>
        public event Func<Task> StartToLoadLevelAsync
        {
            add
            {
                m_startToLoadAsync -= value;
                m_startToLoadAsync += value;
            }
            remove
            {
                m_startToLoadAsync -= value;
            }
        }
        
        /// <summary>
        /// Event when a <see cref="GameLevel"/> start to load.
        /// </summary>
        public event Action StartToLoadLevel
        {
            add
            {
                m_startToLoad -= value;
                m_startToLoad += value;
            }
            remove
            {
                m_startToLoad -= value;
            }
        }
        
        /// <summary>
        /// Event while a <see cref="GameLevel"/> is loading.
        /// </summary>
        public event Action<SceneLoadingInfo> LoadingLevel
        {
            add
            {
                m_loading -= value;
                m_loading += value;
            }
            remove
            {
                m_loading -= value;
            }
        }
        
        /// <summary>
        /// Event when a <see cref="GameLevel"/> start to unload.
        /// </summary>
        public event Action FinishToLoadLevel
        {
            add
            {
                m_finishToLoad -= value;
                m_finishToLoad += value;
            }
            remove
            {
                m_finishToLoad -= value;
            }
        }
        
        /// <summary>
        /// Func call when a <see cref="GameLevel"/> start to unload, can add async callbacks.
        /// </summary>
        public event Func<Task> FinishToLoadLevelAsync
        {
            add
            {
                m_finishToLoadAsync -= value;
                m_finishToLoadAsync += value;
            }
            remove
            {
                m_finishToLoadAsync -= value;
            }
        }
        #endregion

        #region METHODS
        #region LifeCycle
        protected override Task InitAsync(CancellationToken cancellationToken)
        {
            m_saveManager = SaveManager.Instance;
            m_gameLevelManagerSettings = GameLevelManagerSettings.Instance;
            m_loadingScreenManager = LoadingScreenManager.Instance;
            m_gameLevelDatabase = DB.Instance.GetDatabase<GameLevelDatabase>(DBEnum.DB_GameLevels);
            m_sceneInstanceHandles = new List<AsyncOperationHandle<SceneInstance>>();
            
            return Task.CompletedTask;
        }
        
        public override void Delete()
        {
            ReleaseLevelInstances();

            m_saveManager = null;
            m_gameLevelDatabase = null;
            m_levelLoaded = null;
            m_gameLevelManagerSettings = null;
            
            base.Delete();
        }
        #endregion LifeCycle
        
        #region Publics
        /// <summary>
        /// Change the current <see cref="GameLevel"/> in the game, will unload the current loaded <see cref="GameLevel"/>.
        /// </summary>
        public async Task ChangeLevel(DB_GameLevelsEnum gameLevelToLoad, DB_LoadingScreenEnum loadingScreenEnum, bool forceReload = false, bool activateOnLoad = true, int priority = 100)
        {
            if (m_isLoading)
            {
                QRLogger.DebugError<CoreTags.GameLevels>("A scene is already in loading...");
                return;
            }

            m_sceneLoadingInfo = new SceneLoadingInfo()
            {
                GlobalProgress = 0f,
                ProgressDescription = m_gameLevelManagerSettings.LoadingLocalizedString,
            };
            
            m_isLoading = true;
            
            if (m_gameLevelDatabase.TryGetInDatabase(gameLevelToLoad.ToString(), out var levelReferenceGroup))
            {
                // If a level is already loaded, it's managed differently.
                if (forceReload == false && m_levelLoaded != null)
                {
                    if (levelReferenceGroup.name == m_levelLoaded.name)
                    {
                        QRLogger.DebugInfo<CoreTags.GameLevels>($"{gameLevelToLoad.ToString()} is already loaded.", m_gameLevelDatabase);
                        await AlreadyLoadedLevel(loadingScreenEnum);
                        m_isLoading = false;
                        return;
                    }
                }

                var showTask = m_loadingScreenManager.ShowLoadingScreen(loadingScreenEnum);
                var loadingScreenHandle = await showTask;
                var loadingScreen = loadingScreenHandle.LoadingScreen;

                if (m_levelLoaded != null)
                {
                    await UnloadLevel(m_levelLoaded);
                }

                await LoadLevel(gameLevelToLoad, loadingScreen, forceReload, activateOnLoad, priority);
                await m_loadingScreenManager.HideLoadingScreen(loadingScreenHandle);
            }

            m_isLoading = false;
        }

        /// <summary>
        /// Unload the current <see cref="GameLevel"/>.
        /// </summary>
        public Task UnloadCurrentLevel()
        {
            throw new NotImplementedException();
        }

        public void SetAsAlreadyLoadedLevel(GameLevelData gameLevelReferenceGroup)
        {
            m_levelLoaded = gameLevelReferenceGroup;
        }
        #endregion Publics

        #region Privates
        private async Task LoadLevel(DB_GameLevelsEnum gameLevelToLoad, ILoadingScreen loadingScreen, bool forceReload = false, bool activateOnLoad = true, int priority = 100)
        {
            if (m_gameLevelDatabase.TryGetInDatabase(gameLevelToLoad.ToString(), out var levelReferenceGroup))
            {
                if (m_startToLoadAsync != null)
                {
                    await m_startToLoadAsync.Invoke();
                }

                m_startToLoad?.Invoke();
                m_sceneLoadingInfo.SceneLoadingStatus = SceneLoadingStatus.NotLoaded;

                await TryForceReload(forceReload, gameLevelToLoad);
                
                LoadingLevel += loadingScreen.Progress;
                await LoadSceneGroup(levelReferenceGroup, activateOnLoad, priority);

            }
            else
            {
                QRLogger.DebugError<CoreTags.GameLevels>($"Cannot load {gameLevelToLoad.ToString()}, verify SceneDatabase.", m_gameLevelDatabase);
            }
        }
        
        private async Task UnloadLevel(DB_GameLevelsEnum gameLevelToUnload)
        {
            if (m_gameLevelDatabase.TryGetInDatabase(gameLevelToUnload.ToString(), out var foundedObject))
            {
                await UnloadLevel(foundedObject);
            }
        }
        
        private async Task AlreadyLoadedLevel(DB_LoadingScreenEnum loadingScreenEnum)
        {
            var showTask = m_loadingScreenManager.ShowLoadingScreen(loadingScreenEnum);
            var loadingScreenHandle = await showTask;
            var loadingScreen = loadingScreenHandle.LoadingScreen;

            await InitializeLoadedLevel();
            
            if (SaveServiceSettings.Instance.LoadAsyncAfterSceneLoading)
            {
                Load.Current.LoadObjects();
            }

            var hideTask = m_loadingScreenManager.HideLoadingScreen(loadingScreenHandle);
            await hideTask;

            m_sceneLoadingInfo.GlobalProgress = 1f;
            m_sceneLoadingInfo.SceneLoadingStatus = SceneLoadingStatus.SceneAreLoaded;
        }
        
        private async Task<SceneLoadingInfo?> LoadSceneGroup(GameLevelData gameLevelReferenceGroupToLoad, bool activateOnLoad = true, int priority = 100)
        {
            if (m_levelLoaded != null)
            {
                if (m_levelLoaded.GetHashCode() == gameLevelReferenceGroupToLoad.GetHashCode())
                {
                    QRLogger.DebugError<CoreTags.GameLevels>($"{nameof(m_levelLoaded)} already contain {gameLevelReferenceGroupToLoad.ToString()}.");
                    return null;
                }
            }

            m_sceneLoadingInfo.SceneLoadingStatus = SceneLoadingStatus.SceneAreLoading;

            OnLoadingScenes();
            await Task.Delay(TimeSpan.FromSeconds(m_gameLevelManagerSettings.MinimalLoadDurationBefore), CancellationTokenSource.Token);
            
            if (gameLevelReferenceGroupToLoad.GameLevelScenes.IsNotNullOrEmpty())
            {
                var sceneReferenceGroupToLoadCount = gameLevelReferenceGroupToLoad.GameLevelScenes.Length;
                for (var i = 0; i < gameLevelReferenceGroupToLoad.GameLevelScenes.Length; i++)
                {
                    void OnLoadingSubScene(AsyncOperationHandle<SceneInstance> operation)
                    {
                        var currentSceneLoadingProgress = ((i + operation.GetDownloadStatus().Percent) / gameLevelReferenceGroupToLoad.GameLevelScenes.Length) /2f;
                        m_sceneLoadingInfo.GlobalProgress = currentSceneLoadingProgress;
                    }
                    
                    void OnEndLoadingSubScene(AsyncOperationHandle<SceneInstance> operation)
                    {
                        m_sceneLoadingInfo.GlobalProgress = ((i + 1f) / sceneReferenceGroupToLoadCount) /2f;
                    }

                    await LoadScene(gameLevelReferenceGroupToLoad.GameLevelScenes[i], OnLoadingSubScene, OnEndLoadingSubScene, activateOnLoad, priority);
                }
            }

            m_levelLoaded = gameLevelReferenceGroupToLoad;
            QRLogger.DebugInfo<CoreTags.GameLevels>($"{gameLevelReferenceGroupToLoad.name} is loaded.");

            await InitializeLoadedLevel();
            
            if (SaveServiceSettings.Instance.LoadAsyncAfterSceneLoading)
            {
                await m_saveManager.LoadGameAsync();
                Load.Current.LoadObjects();
            }

            await Task.Delay(TimeSpan.FromSeconds(m_gameLevelManagerSettings.MinimalLoadDurationAfter), CancellationTokenSource.Token);

            m_finishToLoad?.Invoke();
            if (m_finishToLoadAsync != null)
            {
                await m_finishToLoadAsync.Invoke();
            }
            
            m_sceneLoadingInfo.SceneLoadingStatus = SceneLoadingStatus.SceneAreLoaded;
            
            return m_sceneLoadingInfo;
        }

        private async void OnLoadingScenes()
        {
            while (m_isLoading)
            {
                m_loading?.Invoke(m_sceneLoadingInfo);
                await Task.Yield();
            }
        }
        
        private async Task UnloadLevel(GameLevelData gameLevelReferenceGroupToUnload)
        {
            if (m_levelLoaded != null)
            {
                if (SaveServiceSettings.Instance.SaveAsyncBeforeSceneLoading)
                {
                    Save.Current.SaveObjects();
                    await m_saveManager.SaveGameAsync();
                }
                
                var levelInitialization = GameLevel.Current;
                if (levelInitialization != null)
                {
                    levelInitialization.UnloadLevel();
                }
                
                foreach (var sceneReference in gameLevelReferenceGroupToUnload.GameLevelScenes)
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

                m_levelLoaded = null;
                QRLogger.DebugInfo<CoreTags.GameLevels>($"{gameLevelReferenceGroupToUnload.name} is unloaded.");   
            }
            else
            {
                QRLogger.DebugInfo<CoreTags.GameLevels>($"{gameLevelReferenceGroupToUnload.name} is already unloaded.");
            }
        }

        private async Task LoadScene(AssetReference sceneObject, Action<AsyncOperationHandle<SceneInstance>> onLoading, Action<AsyncOperationHandle<SceneInstance>> onEndLoading, bool activateOnLoad = true, int priority = 100)
        {
            var loadingSceneObject = sceneObject.LoadSceneAsync(LoadSceneMode.Additive, activateOnLoad, priority);

            if (loadingSceneObject.IsValid())
            {
                m_sceneInstanceHandles.Add(loadingSceneObject);
            }

            m_sceneLoadingInfo.SceneLoadingStatus = SceneLoadingStatus.SceneAreLoading;
            while(!loadingSceneObject.IsDone && !CancellationTokenSource.IsCancellationRequested)
            {
                onLoading?.Invoke(loadingSceneObject);
                await Task.Yield();
            }
            
            onEndLoading?.Invoke(loadingSceneObject);
        }

        private async Task InitializeLoadedLevel()
        {
            var currentGameLevel = GameLevel.Current;

            if (currentGameLevel == null)
            {
                return;
            }
            else
            {
                m_sceneLoadingInfo.SceneLoadingStatus = SceneLoadingStatus.InitializationIsLoading;

                var progression = new Progress<GameLevelLoadingInfo>(value =>
                {
                    m_sceneLoadingInfo.GlobalProgress = .5f + (value.LoadingProgressPercent / 2f);
                });

                var loading = currentGameLevel.LoadLevel(CancellationTokenSource.Token, progression);
                await loading;

                m_sceneLoadingInfo.GlobalProgress = 1f;

                m_sceneLoadingInfo.SceneLoadingStatus = SceneLoadingStatus.InitializationIsDone;
            }
        }

        private async Task TryForceReload(bool forceReload, DB_GameLevelsEnum sceneReferenceGroupToLoad)
        {
            if (forceReload)
            {
                await UnloadLevel(sceneReferenceGroupToLoad);
            }
        }

        private void ReleaseLevelInstances()
        {
            if (m_sceneInstanceHandles != null)
            {
                var handlesCount = m_sceneInstanceHandles.Count;

                if (handlesCount <= 0)
                {
                    return;
                }

                for (var i = 0; i < handlesCount; i++)
                {
                    if (m_sceneInstanceHandles[i].IsValid())
                    {
                        Addressables.ReleaseInstance(m_sceneInstanceHandles[i]);
                    }
                }

                m_sceneInstanceHandles.Clear();
            }
        }
        #endregion Privates
        #endregion METHODS
    }

    [Serializable]
    public struct SceneLoadingInfo
    {
        [ReadOnly][ProgressBar(0f, 1f)] public float GlobalProgress;
        [ReadOnly] public string ProgressDescription;
        [ReadOnly] public SceneLoadingStatus SceneLoadingStatus;
    }

    public enum SceneLoadingStatus
    {
        NotLoaded = 0,
        SceneAreLoading = 1,
        SceneAreLoaded = 2,
        InitializationIsLoading = 3,
        InitializationIsDone = 4,
    }
}
