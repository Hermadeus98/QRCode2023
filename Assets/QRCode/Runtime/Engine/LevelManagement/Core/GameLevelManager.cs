namespace QRCode.Framework.SceneManagement
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Debugging;
    using Engine.Core.Managers;
    using Extensions;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UnityEngine.AddressableAssets;
    using UnityEngine.ResourceManagement.AsyncOperations;
    using UnityEngine.ResourceManagement.ResourceProviders;
    using UnityEngine.SceneManagement;
    using K = Framework.K;

    public class GameLevelManager : SerializedMonoBehaviour, ILevelLoadingManagementService, IManager
    {
        #region FIELDS
        #region Serialized
        [TitleGroup(K.InspectorGroups.Debugging)]
        [SerializeField][ReadOnly] private SceneLoadingInfo m_sceneLoadingInfo;
        #endregion Serialized

        #region Privates
        private bool m_isLoading = false;
        private CancellationTokenSource m_cancellationTokenSource = null;
        private ISaveService m_saveService = null;
        #endregion Privates
        
        #region Statics
        private GameLevelDatabase m_gameLevelDatabase = null;
        private GameLevelDatabase GameLevelDatabase
        {
            get
            {
                if (m_gameLevelDatabase == null)
                {
                    if (DB.Instance.TryGetDatabase<GameLevelDatabase>(DBEnum.DB_Levels, out var sceneDatabase))
                    {
                        m_gameLevelDatabase = sceneDatabase;
                    }
                    else
                    {
                        QRDebug.DebugError(K.DebuggingChannels.LevelManager, $"Cannot load LevelDatabase, verify DB.", gameObject);
                    }
                }

                return m_gameLevelDatabase;
            }
        }

        private LevelLoadingManagerSettings m_levelLoadingManagerSettings = null;
        private LevelLoadingManagerSettings LevelLoadingManagerSettings
        {
            get
            {
                if (m_levelLoadingManagerSettings == null)
                {
                    m_levelLoadingManagerSettings = LevelLoadingManagerSettings.Instance;
                }

                return m_levelLoadingManagerSettings;
            }
        }

        private GameLevelReferenceGroup? m_levelLoaded = null;
        private List<AsyncOperationHandle<SceneInstance>> m_sceneInstanceHandles = new();
        #endregion Statics
        #endregion FIELDS

        #region EVENTS 
        private event Func<Task> m_onStartToLoadAsync;
        public bool IsLoading()
        {
            return m_isLoading;
        }

        public event Func<Task> OnStartToLoadLevelAsync
        {
            add
            {
                m_onStartToLoadAsync -= value;
                m_onStartToLoadAsync += value;
            }
            remove
            {
                m_onStartToLoadAsync -= value;
            }
        }
        
        private event Action m_onStartToLoad;
        public event Action OnStartToLoadLevel
        {
            add
            {
                m_onStartToLoad -= value;
                m_onStartToLoad += value;
            }
            remove
            {
                m_onStartToLoad -= value;
            }
        }
        
        private event Action<SceneLoadingInfo> m_onLoading;
        public event Action<SceneLoadingInfo> OnLoadingLevel
        {
            add
            {
                m_onLoading -= value;
                m_onLoading += value;
            }
            remove
            {
                m_onLoading -= value;
            }
        }
        
        private event Action m_onFinishToLoad;
        public event Action OnFinishToLoadLevel
        {
            add
            {
                m_onFinishToLoad -= value;
                m_onFinishToLoad += value;
            }
            remove
            {
                m_onFinishToLoad -= value;
            }
        }
        
        private event Func<Task> m_onFinishToLoadAsync;
        public event Func<Task> OnFinishToLoadLevelAsync
        {
            add
            {
                m_onFinishToLoadAsync -= value;
                m_onFinishToLoadAsync += value;
            }
            remove
            {
                m_onFinishToLoadAsync -= value;
            }
        }
        #endregion

        #region METHODS

        #region Publics
        public Task InitAsync()
        {
            return Task.CompletedTask;
        }
        
        public async Task<SceneLoadingInfo> ChangeLevel(DB_GameLevelsEnum gameLevelToLoad, DB_LoadingScreenEnum loadingScreenEnum, bool forceReload = false, bool activateOnLoad = true, int priority = 100)
        {
            if (m_isLoading)
            {
                QRDebug.DebugError(K.DebuggingChannels.LevelManager,"A scene is already in loading...");
                return m_sceneLoadingInfo;
            }

            if (GameLevelDatabase.TryGetInDatabase(gameLevelToLoad.ToString(), out var levelReferenceGroup))
            {
                if (m_levelLoaded != null)
                {
                    await UnloadSceneGroup(m_levelLoaded.Value);
                }

                await LoadLevel(gameLevelToLoad, loadingScreenEnum, forceReload, activateOnLoad, priority);
            }

            return m_sceneLoadingInfo;
        }

        public async Task<SceneLoadingInfo> LoadLevel(DB_GameLevelsEnum gameLevelToLoad, DB_LoadingScreenEnum loadingScreenEnum, bool forceReload = false, bool activateOnLoad = true, int priority = 100)
        {
            if (m_isLoading)
            {
                QRDebug.DebugError(K.DebuggingChannels.LevelManager,"A scene is already in loading...");
                return m_sceneLoadingInfo;
            }

            if (GameLevelDatabase.TryGetInDatabase(gameLevelToLoad.ToString(), out var levelReferenceGroup))
            {
                m_isLoading = true;
                var loadingScreen = await UI.GetLoadingScreen(loadingScreenEnum);
                await loadingScreen.Show();

                if (SaveServiceSettings.Instance.SaveAsyncBeforeSceneLoading)
                {
                    await m_saveService.SaveGameAsync();
                }
                
                if (m_onStartToLoadAsync != null)
                {
                    await m_onStartToLoadAsync.Invoke();
                }

                m_onStartToLoad?.Invoke();
                m_sceneLoadingInfo.SceneLoadingStatus = SceneLoadingStatus.NotLoaded;

                await TryForceReload(forceReload, gameLevelToLoad);
                
                OnLoadingLevel += (sceneLoadingInfo) => loadingScreen.Progress(sceneLoadingInfo);
                var sceneLoadingInfo = await LoadSceneGroup(levelReferenceGroup, activateOnLoad, priority);
                
                await loadingScreen.Hide();
                
                return sceneLoadingInfo!.Value;
            }
            else
            {
                QRDebug.DebugError(K.DebuggingChannels.LevelManager, $"Cannot load {gameLevelToLoad.ToString()}, verify SceneDatabase.", GameLevelDatabase);
                return m_sceneLoadingInfo;
            }
        }
        
        public async Task UnloadLevel(DB_GameLevelsEnum gameLevelToUnload)
        {
            if (GameLevelDatabase.TryGetInDatabase(gameLevelToUnload.ToString(), out var foundedObject))
            {
                await UnloadSceneGroup(foundedObject);
            }
        }
        #endregion Publics

        #region LifeCycle
        private void Start()
        {
            m_cancellationTokenSource = new CancellationTokenSource();
            m_saveService = ServiceLocator.Current.Get<ISaveService>();
        }

        private void OnDestroy()
        {
            ReleaseLevelInstances();

            if (m_cancellationTokenSource != null)
            {
                m_cancellationTokenSource.Cancel();
                m_cancellationTokenSource.Dispose();
            }

            m_saveService = null;
            m_gameLevelDatabase = null;
            m_levelLoaded = null;
            m_cancellationTokenSource = null;
            m_levelLoadingManagerSettings = null;
        }
        #endregion LifeCycle
        
        #region Privates
        private async Task<SceneLoadingInfo?> LoadSceneGroup(GameLevelReferenceGroup gameLevelReferenceGroupToLoad, bool activateOnLoad = true, int priority = 100)
        {
            m_sceneLoadingInfo = new SceneLoadingInfo()
            {
                GlobalProgress = 0f,
                ProgressDescription = LevelLoadingManagerSettings.LoadingLocalizedString,
            };

            if (m_levelLoaded != null)
            {
                if (m_levelLoaded.Value.GetHashCode() == gameLevelReferenceGroupToLoad.GetHashCode())
                {
                    QRDebug.DebugError(K.DebuggingChannels.LevelManager, $"{nameof(m_levelLoaded)} already contain {gameLevelReferenceGroupToLoad.ToString()}.");
                    m_isLoading = false;
                    return null;
                }
            }

            m_sceneLoadingInfo.SceneLoadingStatus = SceneLoadingStatus.SceneAreLoading;

            OnLoadingScenes();
            await Task.Delay(TimeSpan.FromSeconds(LevelLoadingManagerSettings.MinimalLoadDurationBefore), m_cancellationTokenSource.Token);
            
            if (gameLevelReferenceGroupToLoad.GameLevel.GameLevelScenes.IsNotNullOrEmpty())
            {
                var sceneReferenceGroupToLoadCount = gameLevelReferenceGroupToLoad.GameLevel.GameLevelScenes.Length;
                for (var i = 0; i < gameLevelReferenceGroupToLoad.GameLevel.GameLevelScenes.Length; i++)
                {
                    void OnLoadingSubScene(AsyncOperationHandle<SceneInstance> operation)
                    {
                        var currentSceneLoadingProgress = ((i + operation.GetDownloadStatus().Percent) / gameLevelReferenceGroupToLoad.GameLevel.GameLevelScenes.Length) /2f;
                        m_sceneLoadingInfo.GlobalProgress = currentSceneLoadingProgress;
                    }
                    
                    void OnEndLoadingSubScene(AsyncOperationHandle<SceneInstance> operation)
                    {
                        m_sceneLoadingInfo.GlobalProgress = ((i + 1f) / sceneReferenceGroupToLoadCount) /2f;
                    }

                    await LoadScene(gameLevelReferenceGroupToLoad.GameLevel.GameLevelScenes[i], OnLoadingSubScene, OnEndLoadingSubScene, activateOnLoad, priority);
                }
            }

            m_levelLoaded = gameLevelReferenceGroupToLoad;
            QRDebug.DebugInfo(K.DebuggingChannels.LevelManager, $"{gameLevelReferenceGroupToLoad.ToString()} is loaded.");

            if (SaveServiceSettings.Instance.LoadAsyncAfterSceneLoading)
            {
                await m_saveService.LoadGameAsync();
            }
            
            await InitializeLoadedLevel();
            
            if (SaveServiceSettings.Instance.LoadAsyncAfterSceneLoading)
            {
                Load.Current.LoadObjects();
            }

            await Task.Delay(TimeSpan.FromSeconds(LevelLoadingManagerSettings.MinimalLoadDurationAfter), m_cancellationTokenSource.Token);

            m_onFinishToLoad?.Invoke();
            if (m_onFinishToLoadAsync != null)
            {
                await m_onFinishToLoadAsync.Invoke();
            }
            
            m_sceneLoadingInfo.SceneLoadingStatus = SceneLoadingStatus.SceneAreLoaded;
            m_isLoading = false;
            
            return m_sceneLoadingInfo;
        }

        private async void OnLoadingScenes()
        {
            while (m_isLoading)
            {
                m_onLoading?.Invoke(m_sceneLoadingInfo);
                await Task.Yield();
            }
        }
        
        private async Task UnloadSceneGroup(GameLevelReferenceGroup gameLevelReferenceGroupToUnload)
        {
            if (m_levelLoaded != null)
            {
                var levelInitialization = LevelInitialization.Current;
                if (levelInitialization != null)
                {
                    levelInitialization.UnloadLevel();
                }
                
                foreach (var sceneReference in gameLevelReferenceGroupToUnload.GameLevel.GameLevelScenes)
                {
                    var unloadSceneOperation = sceneReference.UnLoadScene();

                    while (!unloadSceneOperation.IsDone && !m_cancellationTokenSource.IsCancellationRequested)
                    {
                        await Task.Yield();
                    }
                }

                m_levelLoaded = null;
                QRDebug.DebugInfo(K.DebuggingChannels.LevelManager, $"{gameLevelReferenceGroupToUnload.ToString()} is unloaded.");   
            }
            else
            {
                QRDebug.DebugInfo(K.DebuggingChannels.LevelManager, $"{gameLevelReferenceGroupToUnload.ToString()} is already unloaded.");
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
            var levelInitialization = LevelInitialization.Current;

            if (levelInitialization == null)
            {
                return;
            }
            else
            {
                m_sceneLoadingInfo.SceneLoadingStatus = SceneLoadingStatus.InitializationIsLoading;

                var progression = new Progress<SceneLoadableProgressionInfos>(value =>
                {
                    m_sceneLoadingInfo.GlobalProgress = .5f + (value.LoadingProgressPercent / 2f);
                    m_sceneLoadingInfo.ProgressDescription = value.ProgressionDescription.GetLocalizedString();
                });

                levelInitialization.ForceInitializationFromSceneManager();
                var loading = levelInitialization.LoadLevel(m_cancellationTokenSource.Token, progression);
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
