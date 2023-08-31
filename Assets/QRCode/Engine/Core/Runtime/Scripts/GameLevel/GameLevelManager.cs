namespace QRCode.Engine.Core.GameLevels
{
    using UnityEngine;
    using UnityEngine.AddressableAssets;
    using UnityEngine.ResourceManagement.AsyncOperations;
    using UnityEngine.ResourceManagement.ResourceProviders;
    using UnityEngine.SceneManagement;
    
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    
    using Sirenix.OdinInspector;

    using QRCode.Engine.Debugging;
    using QRCode.Engine.Core.Managers;
    using QRCode.Engine.Core.GameLevels.GeneratedEnums;
    using QRCode.Engine.Core.SaveSystem;
    using QRCode.Engine.Toolbox.Database;
    using QRCode.Engine.Toolbox.Database.GeneratedEnums;
    using QRCode.Engine.Toolbox.Extensions;
    using QRCode.Engine.Toolbox.Pattern.Singleton;
    using QRCode.Engine.Core.UI;
    using QRCode.Engine.Core.UI.LoadingScreen;
    using QRCode.Engine.Core.UI.LoadingScreen.GeneratedEnums;
    using QRCode.Engine.Toolbox.Optimization;
    using Constants = QRCode.Engine.Toolbox.Constants;

    /// <summary>
    /// The <see cref="GameLevelManager"/> will manage the loading/unloading and initialization of game levels.
    /// </summary>
    public class GameLevelManager : MonoBehaviourSingleton<GameLevelManager>, IGameLevelManagementService, IManager, IDeletable
    {
        #region FIELDS
        #region Serialized
        [TitleGroup(Constants.InspectorGroups.Debugging)]
        [SerializeField][ReadOnly] private SceneLoadingInfo m_sceneLoadingInfo;
        #endregion Serialized

        #region Privates
        private GameLevelDatabase m_gameLevelDatabase = null;
        private GameLevelManagerSettings m_gameLevelManagerSettings = null;

        private bool m_isLoading = false;
        private CancellationTokenSource m_cancellationTokenSource = null;
        private ISaveService m_saveService = null;
        private GameLevelData m_levelLoaded = null;
        private List<AsyncOperationHandle<SceneInstance>> m_sceneInstanceHandles = new();
        #endregion Privates
        #endregion FIELDS

        #region EVENTS 
        private event Func<Task> m_startToLoadAsync;

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
        
        private event Action m_startToLoad;
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
        
        private event Action<SceneLoadingInfo> m_loading;
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
        
        private event Action m_finishToLoad;
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
        
        private event Func<Task> m_finishToLoadAsync;
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
        #region Initialization
        public Task InitAsync(CancellationToken cancellationToken)
        {
            m_saveService = SaveManager.Instance;
            m_gameLevelManagerSettings = GameLevelManagerSettings.Instance;
            
            if (DB.Instance.TryGetDatabase<GameLevelDatabase>(DBEnum.DB_Levels, out var sceneDatabase))
            {
                m_gameLevelDatabase = sceneDatabase;
            }
            else
            {
                QRDebug.DebugError(Constants.DebuggingChannels.LevelManager, $"Cannot load LevelDatabase, verify DB.", gameObject);
            }
            
            return Task.CompletedTask;
        }
        #endregion
        
        #region Publics
        /// <summary>
        /// Call this function to change active level. Only one level can be loaded one at one, so, if a level is already loaded, it will be unloaded.
        /// </summary>
        public async Task ChangeLevel(DB_GameLevelsEnum gameLevelToLoad, DB_LoadingScreenEnum loadingScreenEnum, bool forceReload = false, bool activateOnLoad = true, int priority = 100)
        {
            if (m_isLoading)
            {
                QRDebug.DebugError(Constants.DebuggingChannels.LevelManager,"A scene is already in loading...");
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
                        QRDebug.DebugInfo(Constants.DebuggingChannels.LevelManager, $"{gameLevelToLoad.ToString()} is already loaded.", m_gameLevelDatabase);
                        await AlreadyLoadedLevel(loadingScreenEnum);
                        m_isLoading = false;
                        return;
                    }
                }
                
                var loadingScreen = await UI.GetLoadingScreen(loadingScreenEnum);
                await loadingScreen.Show();

                if (m_levelLoaded != null)
                {
                    await UnloadLevel(m_levelLoaded);
                }

                await LoadLevel(gameLevelToLoad, loadingScreen, forceReload, activateOnLoad, priority);
                await loadingScreen.Hide();
            }

            m_isLoading = false;
        }

        public Task UnloadCurrentLevel()
        {
            throw new NotImplementedException();
        }
        
        public bool IsLoading()
        {
            return m_isLoading;
        }

        public void SetAsAlreadyLoadedLevel(GameLevelData gameLevelReferenceGroup)
        {
            m_levelLoaded = gameLevelReferenceGroup;
        }
        
        private async Task LoadLevel(DB_GameLevelsEnum gameLevelToLoad, ILoadingScreen loadingScreen,
            bool forceReload = false, bool activateOnLoad = true, int priority = 100)
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
                QRDebug.DebugError(Constants.DebuggingChannels.LevelManager, $"Cannot load {gameLevelToLoad.ToString()}, verify SceneDatabase.", m_gameLevelDatabase);
            }
        }
        
        private async Task UnloadLevel(DB_GameLevelsEnum gameLevelToUnload)
        {
            if (m_gameLevelDatabase.TryGetInDatabase(gameLevelToUnload.ToString(), out var foundedObject))
            {
                await UnloadLevel(foundedObject);
            }
        }
        #endregion Publics

        #region LifeCycle
        private void Start()
        {
            m_cancellationTokenSource = new CancellationTokenSource();
        }

        private void OnDestroy()
        {
            ReleaseLevelInstances();

            if (m_cancellationTokenSource != null)
            {
                m_cancellationTokenSource.Cancel();
                m_cancellationTokenSource.Dispose();
            }

            m_gameLevelDatabase = null;
            m_levelLoaded = null;
            m_cancellationTokenSource = null;
            m_gameLevelManagerSettings = null;
        }
        #endregion LifeCycle
        
        #region Privates
        private async Task AlreadyLoadedLevel(DB_LoadingScreenEnum loadingScreenEnum)
        {
            var loadingScreen = await UI.GetLoadingScreen(loadingScreenEnum);
            await loadingScreen.Show();

            await InitializeLoadedLevel();
            
            if (SaveServiceSettings.Instance.LoadAsyncAfterSceneLoading)
            {
                Load.Current.LoadObjects();
            }
            
            await loadingScreen.Hide();

            m_sceneLoadingInfo.GlobalProgress = 1f;
            m_sceneLoadingInfo.SceneLoadingStatus = SceneLoadingStatus.SceneAreLoaded;
        }
        
        private async Task<SceneLoadingInfo?> LoadSceneGroup(GameLevelData gameLevelReferenceGroupToLoad, bool activateOnLoad = true, int priority = 100)
        {
            if (m_levelLoaded != null)
            {
                if (m_levelLoaded.GetHashCode() == gameLevelReferenceGroupToLoad.GetHashCode())
                {
                    QRDebug.DebugError(Constants.DebuggingChannels.LevelManager, $"{nameof(m_levelLoaded)} already contain {gameLevelReferenceGroupToLoad.ToString()}.");
                    return null;
                }
            }

            m_sceneLoadingInfo.SceneLoadingStatus = SceneLoadingStatus.SceneAreLoading;

            OnLoadingScenes();
            await Task.Delay(TimeSpan.FromSeconds(m_gameLevelManagerSettings.MinimalLoadDurationBefore), m_cancellationTokenSource.Token);
            
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
            QRDebug.DebugInfo(Constants.DebuggingChannels.LevelManager, $"{gameLevelReferenceGroupToLoad.name} is loaded.");

            await InitializeLoadedLevel();
            
            if (SaveServiceSettings.Instance.LoadAsyncAfterSceneLoading)
            {
                await m_saveService.LoadGameAsync();
                Load.Current.LoadObjects();
            }

            await Task.Delay(TimeSpan.FromSeconds(m_gameLevelManagerSettings.MinimalLoadDurationAfter), m_cancellationTokenSource.Token);

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
                    await m_saveService.SaveGameAsync();
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

                        while (!unloadSceneOperation.IsDone && !m_cancellationTokenSource.IsCancellationRequested)
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
                QRDebug.DebugInfo(Constants.DebuggingChannels.LevelManager, $"{gameLevelReferenceGroupToUnload.name} is unloaded.");   
            }
            else
            {
                QRDebug.DebugInfo(Constants.DebuggingChannels.LevelManager, $"{gameLevelReferenceGroupToUnload.name} is already unloaded.");
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
            while(!loadingSceneObject.IsDone && !m_cancellationTokenSource.IsCancellationRequested)
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

                var loading = currentGameLevel.LoadLevel(m_cancellationTokenSource.Token, progression);
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
        #endregion Privates
        #endregion METHODS

        public void Delete()
        {
            
        }
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
