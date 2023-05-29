namespace QRCode.Framework.SceneManagement
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Debugging;
    using Extensions;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UnityEngine.AddressableAssets;
    using UnityEngine.ResourceManagement.AsyncOperations;
    using UnityEngine.ResourceManagement.ResourceProviders;
    using UnityEngine.SceneManagement;

    public class LevelLoadingManager : SerializedMonoBehaviour, ILevelLoadingManagementService
    {
        #region FIELDS
        #region Serialized
        [TitleGroup(K.InspectorGroups.Debugging)]
        [SerializeField][ReadOnly] private SceneLoadingInfo m_sceneLoadingInfo;
        #endregion Serialized

        #region Privates
        private bool m_isLoading = false;
        private  CancellationTokenSource m_cancellationTokenSource = null;
        private ISaveService m_saveService = null;
        #endregion Privates
        
        #region Statics
        private LevelDatabase m_levelDatabase = null;
        private LevelDatabase LevelDatabase
        {
            get
            {
                if (m_levelDatabase == null)
                {
                    if (DB.Instance.TryGetDatabase<LevelDatabase>(DBEnum.DB_Levels, out var sceneDatabase))
                    {
                        m_levelDatabase = sceneDatabase;
                    }
                    else
                    {
                        QRDebug.DebugError(K.DebuggingChannels.SceneManager, $"Cannot load SceneDatabase, verify DB.", gameObject);
                    }
                }

                return m_levelDatabase;
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
        
        private List<LevelReferenceGroup> m_loadedLevelGroup = new List<LevelReferenceGroup>();
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
        public async Task<SceneLoadingInfo> ChangeLevel(DB_LevelsEnum levelToLoad, DB_LoadingScreenEnum loadingScreenEnum, bool forceReload = false, bool activateOnLoad = true, int priority = 100)
        {
            if (m_isLoading)
            {
                QRDebug.DebugError(K.DebuggingChannels.SceneManager,"A scene is already in loading...");
                return m_sceneLoadingInfo;
            }

            if (LevelDatabase.TryGetInDatabase(levelToLoad.ToString(), out var levelReferenceGroup))
            {
                if (m_loadedLevelGroup.Count > 0)
                {
                    await UnloadSceneGroup(m_loadedLevelGroup[0]);
                }
                
                await LoadLevel(levelToLoad, loadingScreenEnum, forceReload, activateOnLoad, priority);
            }

            return m_sceneLoadingInfo;
        }

        public async Task<SceneLoadingInfo> LoadLevel(DB_LevelsEnum levelToLoad, DB_LoadingScreenEnum loadingScreenEnum, bool forceReload = false, bool activateOnLoad = true, int priority = 100)
        {
            if (m_isLoading)
            {
                QRDebug.DebugError(K.DebuggingChannels.SceneManager,"A scene is already in loading...");
                return m_sceneLoadingInfo;
            }

            if (LevelDatabase.TryGetInDatabase(levelToLoad.ToString(), out var levelReferenceGroup))
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

                await TryForceReload(forceReload, levelToLoad);
                
                OnLoadingLevel += (sceneLoadingInfo) => loadingScreen.Progress(sceneLoadingInfo);
                var sceneLoadingInfo = await LoadSceneGroup(levelReferenceGroup, activateOnLoad, priority);
                
                await loadingScreen.Hide();
                
                return sceneLoadingInfo!.Value;
            }
            else
            {
                QRDebug.DebugError(K.DebuggingChannels.SceneManager, $"Cannot load {levelToLoad.ToString()}, verify SceneDatabase.", LevelDatabase);
                return m_sceneLoadingInfo;
            }
        }
        
        public async Task UnloadLevel(DB_LevelsEnum levelToUnload)
        {
            if (LevelDatabase.TryGetInDatabase(levelToUnload.ToString(), out var foundedObject))
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
            m_cancellationTokenSource?.Cancel();
        }
        #endregion LifeCycle
        
        #region Privates
        private async Task<SceneLoadingInfo?> LoadSceneGroup(LevelReferenceGroup levelReferenceGroupToLoad, bool activateOnLoad = true, int priority = 100)
        {
            m_sceneLoadingInfo = new SceneLoadingInfo()
            {
                GlobalProgress = 0f,
                ProgressDescription = LevelLoadingManagerSettings.LoadingLocalizedString,
            };

            if (m_loadedLevelGroup.Contains(levelReferenceGroupToLoad))
            {
                QRDebug.DebugError(K.DebuggingChannels.SceneManager, $"{nameof(m_loadedLevelGroup)} already contain {levelReferenceGroupToLoad.ToString()}.");
                m_isLoading = false;
                return null;
            }

            m_sceneLoadingInfo.SceneLoadingStatus = SceneLoadingStatus.SceneAreLoading;

            OnLoadingScenes();
            await Task.Delay(TimeSpan.FromSeconds(LevelLoadingManagerSettings.MinimalLoadDurationBefore), m_cancellationTokenSource.Token);
            
            if (levelReferenceGroupToLoad.Levels.IsNotNullOrEmpty())
            {
                var sceneReferenceGroupToLoadCount = levelReferenceGroupToLoad.Levels.Length;
                for (var i = 0; i < levelReferenceGroupToLoad.Levels.Length; i++)
                {
                    void OnLoadingSubScene(AsyncOperationHandle<SceneInstance> operation)
                    {
                        var currentSceneLoadingProgress = ((i + operation.GetDownloadStatus().Percent) / levelReferenceGroupToLoad.Levels.Length) /2f;
                        m_sceneLoadingInfo.GlobalProgress = currentSceneLoadingProgress;
                    }
                    
                    void OnEndLoadingSubScene(AsyncOperationHandle<SceneInstance> operation)
                    {
                        m_sceneLoadingInfo.GlobalProgress = ((i + 1f) / sceneReferenceGroupToLoadCount) /2f;
                    }

                    await LoadScene(levelReferenceGroupToLoad.Levels[i], OnLoadingSubScene, OnEndLoadingSubScene, activateOnLoad, priority);
                }
            }

            m_loadedLevelGroup.Add(levelReferenceGroupToLoad);
            QRDebug.DebugInfo(K.DebuggingChannels.SceneManager, $"{levelReferenceGroupToLoad.ToString()} is loaded.");

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
        
        private async Task UnloadSceneGroup(LevelReferenceGroup levelReferenceGroupToUnload)
        {
            if (m_loadedLevelGroup.Contains(levelReferenceGroupToUnload))
            {
                var levelInitialization = LevelInitialization.Current;
                if (levelInitialization != null)
                {
                    levelInitialization.UnloadLevel();
                }
                
                foreach (var sceneReference in levelReferenceGroupToUnload.Levels)
                {
                    var unloadSceneOperation = sceneReference.UnLoadScene();

                    while (!unloadSceneOperation.IsDone && !m_cancellationTokenSource.IsCancellationRequested)
                    {
                        await Task.Yield();
                    }
                }

                m_loadedLevelGroup.Remove(levelReferenceGroupToUnload);
                QRDebug.DebugInfo(K.DebuggingChannels.SceneManager, $"{levelReferenceGroupToUnload.ToString()} is unloaded.");   
            }
            else
            {
                QRDebug.DebugInfo(K.DebuggingChannels.SceneManager, $"{levelReferenceGroupToUnload.ToString()} is already unloaded.");
            }
        }

        private async Task LoadScene(AssetReference sceneObject, Action<AsyncOperationHandle<SceneInstance>> onLoading, Action<AsyncOperationHandle<SceneInstance>> onEndLoading, bool activateOnLoad = true, int priority = 100)
        {
            var loadingSceneObject = sceneObject.LoadSceneAsync(LoadSceneMode.Additive, activateOnLoad, priority);
            
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

        private async Task TryForceReload(bool forceReload, DB_LevelsEnum sceneReferenceGroupToLoad)
        {
            if (forceReload)
            {
                await UnloadLevel(sceneReferenceGroupToLoad);
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
