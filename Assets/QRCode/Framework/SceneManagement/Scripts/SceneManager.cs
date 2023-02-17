namespace QRCode.Framework.SceneManagement
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Debugging;
    using Extensions;
    using Singleton;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UnityEngine.AddressableAssets;
    using UnityEngine.ResourceManagement.AsyncOperations;
    using UnityEngine.ResourceManagement.ResourceProviders;
    using UnityEngine.SceneManagement;

    public class SceneManager : MonoBehaviourSingleton<SceneManager>
    {
        #region FIELDS
        #region Serialized
        [TitleGroup(K.InspectorGroups.Settings)]
        [SerializeField] private float m_minimalLoadDurationBefore = .5f;
        
        [TitleGroup(K.InspectorGroups.Settings)]
        [SerializeField] private float m_minimalLoadDurationAfter = .5f;

        [TitleGroup(K.InspectorGroups.Settings)] 
        [SerializeField] private string m_loadingSceneProgressionDescription = "Loading...";
        
        [TitleGroup(K.InspectorGroups.Debugging)]
        [SerializeField][ReadOnly] private SceneLoadingInfo m_sceneLoadingInfo;
        #endregion Serialized

        #region Privates
        private bool m_isLoading = false;
        private  CancellationTokenSource m_cancellationTokenSource = null;
        #endregion Privates

        #region Statics
        private static SceneDatabase m_sceneDatabase = null;
        private static SceneDatabase SceneDatabase
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
                        QRDebug.DebugError(K.DebuggingChannels.SceneManager, $"Cannot load SceneDatabase, verify DB.", Instance);
                    }
                }

                return m_sceneDatabase;
            }
        }
        private static List<SceneDatabase.SceneReferenceGroup> m_loadedSceneGroup = new List<SceneDatabase.SceneReferenceGroup>();
        #endregion Statics
        #endregion FIELDS

        #region EVENTS 
        private static event Func<Task> m_onStartToLoadAsync;
        public static event Func<Task> OnStartToLoadAsync
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
        
        private static event Action m_onStartToLoad;
        public static event Action OnStartToLoad
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
        
        private static event Action<SceneLoadingInfo> m_onLoading;
        public static event Action<SceneLoadingInfo> OnLoading
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
        
        private static event Action m_onFinishToLoad;
        public static event Action OnFinishToLoad
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
        
        private static event Func<Task> m_onFinishToLoadAsync;
        public static event Func<Task> OnFinishToLoadAsync
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
        #region Initialization
        protected override void OnInitialize()
        {
            base.OnInitialize();
            m_cancellationTokenSource = new CancellationTokenSource();
        }
        #endregion

        #region Publics
        public async Task<SceneLoadingInfo> LoadSceneGroup(DB_SceneEnum sceneReferenceGroupToLoad, DB_LoadingScreenEnum loadingScreenEnum, bool forceReload = false, bool activateOnLoad = true, int priority = 100)
        {
            if (m_isLoading)
            {
                QRDebug.DebugError(K.DebuggingChannels.SceneManager,"A scene is already in loading...");
                return m_sceneLoadingInfo;
            }

            if (SceneDatabase.TryGetInDatabase(sceneReferenceGroupToLoad.ToString(), out var sceneReferenceGroup))
            {
                var loadingScreen = UI.GetLoadingScreen(loadingScreenEnum);
                await loadingScreen.Show();

                if (forceReload)
                {
                    await UnloadSceneGroup(sceneReferenceGroupToLoad);
                }
                
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
        
        public async Task UnloadSceneGroup(DB_SceneEnum sceneReferenceGroupToUnload)
        {
            if (m_sceneDatabase.TryGetInDatabase(sceneReferenceGroupToUnload.ToString(), out var foundedObject))
            {
                await UnloadSceneGroup(foundedObject);
            }
        }
        #endregion Publics

        #region LifeCycle
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
                ProgressDescription = m_loadingSceneProgressionDescription,
            };

            if (m_loadedSceneGroup.Contains(sceneReferenceGroupToLoad))
            {
                QRDebug.DebugError(K.DebuggingChannels.SceneManager, $"{nameof(m_loadedSceneGroup)} already contain {sceneReferenceGroupToLoad.ToString()}.");
                m_isLoading = false;
                return null;
            }

            m_isLoading = true;
            await Task.Delay(TimeSpan.FromSeconds(m_minimalLoadDurationBefore), m_cancellationTokenSource.Token);
            OnLoadingScenes();

            if (m_onStartToLoadAsync != null)
            {
                await m_onStartToLoadAsync.Invoke();
            }
            
            m_onStartToLoad?.Invoke();
            m_sceneLoadingInfo.SceneLoadingStatus = SceneLoadingStatus.NotLoaded;
            m_sceneLoadingInfo.SceneLoadingStatus = SceneLoadingStatus.SceneAreLoading;

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
            m_sceneLoadingInfo.SceneLoadingStatus = SceneLoadingStatus.SceneAreLoaded;

            var initializeTask = InitializeLoadedScene();
            await initializeTask;

            await Task.Delay(TimeSpan.FromSeconds(m_minimalLoadDurationAfter), m_cancellationTokenSource.Token);

            m_onFinishToLoad?.Invoke();
            if (m_onFinishToLoadAsync != null)
            {
                await m_onFinishToLoadAsync.Invoke();
            }

            m_isLoading = false;
            QRDebug.DebugInfo(K.DebuggingChannels.SceneManager, $"{sceneReferenceGroupToLoad.ToString()} is loaded.");
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

        private async Task InitializeLoadedScene()
        {
            var initialization = Initialization.Current;

            if (initialization == null)
            {
                return;
            }
            else
            {
                m_sceneLoadingInfo.SceneLoadingStatus = SceneLoadingStatus.InitializationIsLoading;

                var progression = new Progress<SceneLoadableProgressionInfos>(value =>
                {
                    m_sceneLoadingInfo.GlobalProgress = .5f + (value.LoadingProgressPercent / 2f);
                    m_sceneLoadingInfo.ProgressDescription = value.ProgressionDescription;
                });
                
                var loading = initialization.Load(m_cancellationTokenSource.Token, progression);
                await loading;

                m_sceneLoadingInfo.GlobalProgress = 1f;

                m_sceneLoadingInfo.SceneLoadingStatus = SceneLoadingStatus.InitializationIsDone;
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
