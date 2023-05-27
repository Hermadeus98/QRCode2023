namespace QRCode.Framework.SceneManagement
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Debugging;
    using Extensions;
    using Game;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UnityEngine.AddressableAssets;
    using UnityEngine.ResourceManagement.AsyncOperations;
    using UnityEngine.ResourceManagement.ResourceProviders;
    using UnityEngine.SceneManagement;

    public class SceneManager : SerializedMonoBehaviour, ISceneManagementService
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
        private SceneDatabase m_sceneDatabase = null;
        private SceneDatabase SceneDatabase
        {
            get
            {
                if (m_sceneDatabase == null)
                {
                    if (DB.Instance.TryGetDatabase<SceneDatabase>(DBEnum.DB_Scene, out var sceneDatabase))
                    {
                        m_sceneDatabase = sceneDatabase;
                    }
                    else
                    {
                        QRDebug.DebugError(K.DebuggingChannels.SceneManager, $"Cannot load SceneDatabase, verify DB.", gameObject);
                    }
                }

                return m_sceneDatabase;
            }
        }

        private SceneManagerSettings m_sceneManagerSettings = null;
        private SceneManagerSettings SceneManagerSettings
        {
            get
            {
                if (m_sceneManagerSettings == null)
                {
                    m_sceneManagerSettings = SceneManagerSettings.Instance;
                }

                return m_sceneManagerSettings;
            }
        }
        
        private List<SceneDatabase.SceneReferenceGroup> m_loadedSceneGroup = new List<SceneDatabase.SceneReferenceGroup>();
        #endregion Statics
        #endregion FIELDS

        #region EVENTS 
        private event Func<Task> m_onStartToLoadAsync;
        public bool IsLoading()
        {
            return m_isLoading;
        }

        public event Func<Task> OnStartToLoadAsync
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
        public event Action OnStartToLoad
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
        public event Action<SceneLoadingInfo> OnLoading
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
        public event Action OnFinishToLoad
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
        public event Func<Task> OnFinishToLoadAsync
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
        public async Task<SceneLoadingInfo> LoadLevel(DB_SceneEnum sceneReferenceGroupToLoad, DB_LoadingScreenEnum loadingScreenEnum, bool forceReload = false, bool activateOnLoad = true, int priority = 100)
        {
            if (m_isLoading)
            {
                QRDebug.DebugError(K.DebuggingChannels.SceneManager,"A scene is already in loading...");
                return m_sceneLoadingInfo;
            }

            if (SceneDatabase.TryGetInDatabase(sceneReferenceGroupToLoad.ToString(), out var sceneReferenceGroup))
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

                await TryForceReload(forceReload, sceneReferenceGroupToLoad);
                
                OnLoading += (sceneLoadingInfo) => loadingScreen.Progress(sceneLoadingInfo);
                var sceneLoadingInfo = await LoadSceneGroup(sceneReferenceGroup, activateOnLoad, priority);
                
                await loadingScreen.Hide();
                
                return sceneLoadingInfo!.Value;
            }
            else
            {
                QRDebug.DebugError(K.DebuggingChannels.SceneManager, $"Cannot load {sceneReferenceGroupToLoad.ToString()}, verify SceneDatabase.", SceneDatabase);
                return m_sceneLoadingInfo;
            }
        }
        
        public async Task UnloadLevel(DB_SceneEnum sceneReferenceGroupToUnload)
        {
            if (SceneDatabase.TryGetInDatabase(sceneReferenceGroupToUnload.ToString(), out var foundedObject))
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
        private async Task<SceneLoadingInfo?> LoadSceneGroup(SceneDatabase.SceneReferenceGroup sceneReferenceGroupToLoad, bool activateOnLoad = true, int priority = 100)
        {
            m_sceneLoadingInfo = new SceneLoadingInfo()
            {
                GlobalProgress = 0f,
                ProgressDescription = SceneManagerSettings.LoadingLocalizedString,
            };

            if (m_loadedSceneGroup.Contains(sceneReferenceGroupToLoad))
            {
                QRDebug.DebugError(K.DebuggingChannels.SceneManager, $"{nameof(m_loadedSceneGroup)} already contain {sceneReferenceGroupToLoad.ToString()}.");
                m_isLoading = false;
                return null;
            }

            m_sceneLoadingInfo.SceneLoadingStatus = SceneLoadingStatus.SceneAreLoading;

            OnLoadingScenes();
            await Task.Delay(TimeSpan.FromSeconds(SceneManagerSettings.MinimalLoadDurationBefore), m_cancellationTokenSource.Token);
            
            if (sceneReferenceGroupToLoad.Scenes.IsNotNullOrEmpty())
            {
                var sceneReferenceGroupToLoadCount = sceneReferenceGroupToLoad.Scenes.Length;
                for (var i = 0; i < sceneReferenceGroupToLoad.Scenes.Length; i++)
                {
                    void OnLoadingSubScene(AsyncOperationHandle<SceneInstance> operation)
                    {
                        var currentSceneLoadingProgress = ((i + operation.GetDownloadStatus().Percent) / sceneReferenceGroupToLoad.Scenes.Length) /2f;
                        m_sceneLoadingInfo.GlobalProgress = currentSceneLoadingProgress;
                    }
                    
                    void OnEndLoadingSubScene(AsyncOperationHandle<SceneInstance> operation)
                    {
                        m_sceneLoadingInfo.GlobalProgress = ((i + 1f) / sceneReferenceGroupToLoadCount) /2f;
                    }

                    await LoadScene(sceneReferenceGroupToLoad.Scenes[i], OnLoadingSubScene, OnEndLoadingSubScene, activateOnLoad, priority);
                }
            }

            m_loadedSceneGroup.Add(sceneReferenceGroupToLoad);
            QRDebug.DebugInfo(K.DebuggingChannels.SceneManager, $"{sceneReferenceGroupToLoad.ToString()} is loaded.");

            if (SaveServiceSettings.Instance.LoadAsyncAfterSceneLoading)
            {
                await m_saveService.LoadGameAsync();
            }
            
            await InitializeLoadedLevel();
            
            if (SaveServiceSettings.Instance.LoadAsyncAfterSceneLoading)
            {
                Load.Current.LoadObjects();
            }

            await Task.Delay(TimeSpan.FromSeconds(SceneManagerSettings.MinimalLoadDurationAfter), m_cancellationTokenSource.Token);

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
        
        private async Task UnloadSceneGroup(SceneDatabase.SceneReferenceGroup sceneReferenceGroupToUnload)
        {
            if (m_loadedSceneGroup.Contains(sceneReferenceGroupToUnload))
            {
                var levelInitialization = LevelInitialization.Current;
                if (levelInitialization != null)
                {
                    levelInitialization.UnloadLevel();
                }
                
                foreach (var sceneReference in sceneReferenceGroupToUnload.Scenes)
                {
                    var unloadSceneOperation = sceneReference.UnLoadScene();

                    while (!unloadSceneOperation.IsDone && !m_cancellationTokenSource.IsCancellationRequested)
                    {
                        await Task.Yield();
                    }
                }

                m_loadedSceneGroup.Remove(sceneReferenceGroupToUnload);
                QRDebug.DebugInfo(K.DebuggingChannels.SceneManager, $"{sceneReferenceGroupToUnload.ToString()} is unloaded.");   
            }
            else
            {
                QRDebug.DebugInfo(K.DebuggingChannels.SceneManager, $"{sceneReferenceGroupToUnload.ToString()} is already unloaded.");
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

        private async Task TryForceReload(bool forceReload, DB_SceneEnum sceneReferenceGroupToLoad)
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
