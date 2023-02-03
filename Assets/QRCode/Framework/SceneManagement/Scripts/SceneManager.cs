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
        private static CancellationTokenSource m_cancellationTokenSource = null;
        
        private static SceneDatabase m_sceneDatabase = null;
        private static SceneDatabase SceneDatabase
        {
            get
            {
                if (m_sceneDatabase == null)
                {
                    if (DB.Instance.TryGetDatabase<SceneDatabase>("Database_Scenes", out var sceneDatabase))
                    {
                        m_sceneDatabase = sceneDatabase;
                    }
                    else
                    {
                        QRDebug.DebugError(K.DebugChannels.SceneManager, $"Cannot load SceneDatabase, verify DB.", Instance);
                    }
                }

                return m_sceneDatabase;
            }
        }

        [SerializeField][ReadOnly]
        private SceneLoadingInfo m_sceneLoadingInfo = new SceneLoadingInfo();
        public static SceneLoadingInfo SceneLoadingInfo
        {
            get
            {
                return Instance.m_sceneLoadingInfo;
            }
        }
        
        private static List<SceneDatabase.SceneReferenceGroup> m_loadedSceneGroup = new List<SceneDatabase.SceneReferenceGroup>();

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
        
        protected override void OnInitialize()
        {
            base.OnInitialize();
            m_cancellationTokenSource = new CancellationTokenSource();
        }

        public async Task<SceneLoadingInfo> LoadSceneGroup(SceneReferenceGroupEnum sceneReferenceGroupToLoad,
            bool activateOnLoad = true, int priority = 100)
        {
            if (SceneDatabase.TryGetSceneReferenceGroup(sceneReferenceGroupToLoad, out var sceneReferenceGroup))
            {
                return await LoadSceneGroup(sceneReferenceGroup, activateOnLoad, priority);
            }
            else
            {
                QRDebug.DebugError(K.DebugChannels.SceneManager, $"Cannot load {sceneReferenceGroupToLoad.ToString()}, verify SceneDatabase.", SceneDatabase);
                return m_sceneLoadingInfo;
            }
        }
        
        public async Task<SceneLoadingInfo> LoadSceneGroup(SceneDatabase.SceneReferenceGroup sceneReferenceGroupToLoad, bool activateOnLoad = true, int priority = 100)
        {
            if (m_loadedSceneGroup.Contains(sceneReferenceGroupToLoad))
            {
                QRDebug.DebugError(K.DebugChannels.SceneManager, $"{nameof(m_loadedSceneGroup)} already contain {sceneReferenceGroupToLoad.SceneReferenceGroupName}.");
                return m_sceneLoadingInfo;
            }

            if (m_onStartToLoadAsync != null)
            {
                await m_onStartToLoadAsync.Invoke();
            }
            
            m_onStartToLoad?.Invoke();
            m_sceneLoadingInfo.SceneLoadingStatus = SceneLoadingStatus.NotLoaded;
            var totalProgress = new float[sceneReferenceGroupToLoad.Scenes.Length];
            m_sceneLoadingInfo.SceneLoadingStatus = SceneLoadingStatus.IsLoading;

            if (sceneReferenceGroupToLoad.Scenes.IsNotNullOrEmpty())
            {
                for (int i = 0; i < sceneReferenceGroupToLoad.Scenes.Length; i++)
                {
                    void OnLoadingSubScene(AsyncOperationHandle<SceneInstance> operation)
                    {
                        totalProgress[i] = operation.GetDownloadStatus().Percent;
                        m_sceneLoadingInfo.GlobalProgress = totalProgress.GetAverage();
                    }
                    
                    void OnEndLoadingSubScene(AsyncOperationHandle<SceneInstance> operation)
                    {
                        totalProgress[i] = operation.GetDownloadStatus().Percent;
                    }

                    await LoadScene(sceneReferenceGroupToLoad.Scenes[i], OnLoadingSubScene, OnEndLoadingSubScene, activateOnLoad, priority);
                    totalProgress[i] = 1f;
                }
            }

            m_sceneLoadingInfo.SceneLoadingStatus = SceneLoadingStatus.IsLoaded;
            m_loadedSceneGroup.Add(sceneReferenceGroupToLoad);
            m_onFinishToLoad?.Invoke();
            if (m_onFinishToLoadAsync != null)
            {
                await m_onFinishToLoadAsync.Invoke();
            }
            QRDebug.DebugInfo(K.DebugChannels.SceneManager, $"{sceneReferenceGroupToLoad.SceneReferenceGroupName} is loaded.");
            return m_sceneLoadingInfo;
        }

        public async Task UnloadSceneGroup(SceneReferenceGroupEnum sceneReferenceGroupToUnload)
        {
            if (SceneDatabase.TryGetSceneReferenceGroup(sceneReferenceGroupToUnload, out var sceneReferenceGroup))
            {
                if (sceneReferenceGroup.Scenes.IsNotNullOrEmpty())
                {
                    for (int i = 0; i < sceneReferenceGroup.Scenes.Length; i++)
                    {
                        var unloadSceneOperation = sceneReferenceGroup.Scenes[i].UnLoadScene();

                        while (!unloadSceneOperation.IsDone && !m_cancellationTokenSource.IsCancellationRequested)
                        {
                            await Task.Yield();
                        }
                    }
                }

                m_loadedSceneGroup.Remove(sceneReferenceGroup);
                QRDebug.DebugInfo(K.DebugChannels.SceneManager, $"{sceneReferenceGroupToUnload.ToString()} is unloaded.");
            }
            else
            {
                QRDebug.DebugError(K.DebugChannels.SceneManager, $"{nameof(m_loadedSceneGroup)} don't contain {sceneReferenceGroupToUnload.ToString()}.");
            }
        }

        private async Task LoadScene(AssetReference sceneObject, Action<AsyncOperationHandle<SceneInstance>> onLoading, Action<AsyncOperationHandle<SceneInstance>> onEndLoading, bool activateOnLoad = true, int priority = 100)
        {
            var loadingSceneObject = sceneObject.LoadSceneAsync(LoadSceneMode.Additive, activateOnLoad, priority);
            
            m_sceneLoadingInfo.SceneLoadingStatus = SceneLoadingStatus.IsLoading;
            while(!loadingSceneObject.IsDone && !m_cancellationTokenSource.IsCancellationRequested)
            {
                var downloadPercent = loadingSceneObject.GetDownloadStatus().Percent;
                m_sceneLoadingInfo.CurrentLoadingProgress = downloadPercent;
                onLoading?.Invoke(loadingSceneObject);
                m_onLoading?.Invoke(SceneLoadingInfo);
                await Task.Yield();
            }

            onEndLoading?.Invoke(loadingSceneObject);
        }

        private Scene[] GetActiveScenes()
        {
            var sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCount;
            Scene[] scenesToReturn = new Scene[sceneCount];

            for (var i = 0; i < sceneCount; i++)
            {
                scenesToReturn[i] = UnityEngine.SceneManagement.SceneManager.GetSceneAt(i);
            }

            return scenesToReturn;
        }
        
        private void OnDestroy()
        {
            m_cancellationTokenSource?.Cancel();
        }
    }

    [Serializable]
    public struct SceneLoadingInfo
    {
        [ProgressBar(0, 1)] [ReadOnly] public float GlobalProgress;
        [ProgressBar(0, 1)] [ReadOnly] public float CurrentLoadingProgress;
        [ReadOnly] public SceneLoadingStatus SceneLoadingStatus;
    }

    public enum SceneLoadingStatus
    {
        NotLoaded = 0,
        IsLoading = 1,
        IsLoaded = 2,
    }
}
