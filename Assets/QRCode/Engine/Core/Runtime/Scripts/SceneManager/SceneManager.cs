namespace QRCode.Engine.Core.SceneManagement
{
    using UnityEngine.AddressableAssets;
    using UnityEngine.ResourceManagement.AsyncOperations;
    using UnityEngine.ResourceManagement.ResourceProviders;
    using UnityEngine.SceneManagement;
    using UnityEngine;
    
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    
    using QRCode.Engine.Debugging;
    using QRCode.Engine.Core.SceneManagement.GeneratedEnum;
    using QRCode.Engine.Core.Managers;
    using QRCode.Engine.Toolbox.Database;
    using QRCode.Engine.Toolbox.Database.GeneratedEnums;
    using QRCode.Engine.Toolbox.Pattern.Singleton;
    using Constants = QRCode.Engine.Toolbox.Constants;
    using Timer = System.Timers.Timer;

    public class SceneManager : MonoBehaviourSingleton<SceneManager>, IManager
    {
        private Dictionary<string, AsyncOperationHandle<SceneInstance>> m_loadingOperationHandles = null;
        private List<Scene> m_loadedScenes = null;
        private CancellationTokenSource m_cancellationTokenSource = null;
        
        private SceneDatabase m_sceneDatabase = null;
        private SceneDatabase SceneDatabase
        {
            get
            {
                if (m_sceneDatabase == null)
                {
                    if(DB.Instance.TryGetDatabase<SceneDatabase>(DBEnum.DB_Scenes, out var sceneDatabase))
                    {
                        m_sceneDatabase = sceneDatabase;
                    }
                    else
                    {
                        QRDebug.DebugError(Constants.DebuggingChannels.SceneManager, $"Cannot load SceneDatabase, verify DB.");
                    }
                }

                return m_sceneDatabase;
            }
        }
        
        public Task InitAsync(CancellationToken cancellationToken)
        {
            m_loadingOperationHandles = new Dictionary<string, AsyncOperationHandle<SceneInstance>>();
            m_cancellationTokenSource = new CancellationTokenSource();
            m_loadedScenes = new List<Scene>();
            
            RegisterLoadedScenes();

            return Task.CompletedTask;
        }

        private void OnDestroy()
        {
            Delete();
        }

        public void Delete()
        {
            if (m_loadingOperationHandles != null)
            {
                foreach (var asyncOperationHandle in m_loadingOperationHandles.Values)
                {
                    if (asyncOperationHandle.IsValid() == true)
                    {
                        Addressables.ReleaseInstance(asyncOperationHandle);
                    }
                }
                
                m_loadingOperationHandles.Clear();
                m_loadingOperationHandles = null;
            }

            if (m_cancellationTokenSource != null)
            {
                m_cancellationTokenSource.Cancel();
                m_cancellationTokenSource.Dispose();
                m_cancellationTokenSource = null;
            }
        }
        
        public async Task LoadScene(DB_ScenesEnum sceneToLoad, IProgress<SceneLoadingInfo> sceneLoadingProgress, LoadSceneMode loadSceneMode = LoadSceneMode.Additive)
        {
            var sceneName = sceneToLoad.ToString();
            if (SceneDatabase.TryGetInDatabase(sceneName, out var sceneReference))
            {
                await LoadSceneInternal(sceneName, sceneReference,sceneLoadingProgress, loadSceneMode);
            }
            else
            {
                throw new SceneManagementException($"Cannot find {sceneToLoad.ToString()} in {m_sceneDatabase.name}");
            }
        }
        
        public async Task UnLoadScene(DB_ScenesEnum sceneToUnload)
        {
            var sceneName = sceneToUnload.ToString();
            if (SceneDatabase.TryGetInDatabase(sceneName, out _))
            {
                await UnloadSceneInternal(sceneName);
            }
            else
            {
                throw new SceneManagementException($"Cannot find {sceneToUnload.ToString()} in {m_sceneDatabase.name}");
            }
        }

        private async Task LoadSceneInternal(string sceneToLoad, SceneReference sceneReference, [NotNull] IProgress<SceneLoadingInfo> sceneLoadingProgress, LoadSceneMode loadSceneMode = LoadSceneMode.Additive)
        {
            if (SceneIsAlreadyLoaded(sceneToLoad))
            {
                QRDebug.DebugError(Constants.DebuggingChannels.SceneManager, $"The scene {sceneToLoad} is already loaded.");
                return;
            }

            var loadOperationHandle = sceneReference.Scene.LoadSceneAsync(loadSceneMode);
            
#if QRCODE_TRC_CHECK
            var initialTime = Time.time;
            var elapsedTime = 0.0f;
            loadOperationHandle.Completed += _ =>
            {
                var finalTime = Time.time;
                elapsedTime = finalTime - initialTime;
            };
#endif

            var sceneLoadingInfoData = new SceneLoadingInfo(loadOperationHandle.GetDownloadStatus());
            sceneLoadingProgress.Report(sceneLoadingInfoData);

            await loadOperationHandle.Task;

            sceneLoadingInfoData = new SceneLoadingInfo(loadOperationHandle.GetDownloadStatus());
            sceneLoadingProgress.Report(sceneLoadingInfoData);
            
#if QRCODE_TRC_CHECK
            CheckTrcSceneLoadingDuration(Mathf.CeilToInt(elapsedTime));
#endif
            
            var sceneInstance = loadOperationHandle.Result;
            
            m_loadedScenes.Add(sceneInstance.Scene);
            m_loadingOperationHandles.Add(sceneToLoad, loadOperationHandle);
        }

        private async Task UnloadSceneInternal(string sceneToUnload)
        {
            var scene = UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneToUnload);
                
            if (m_loadingOperationHandles.TryGetValue(sceneToUnload, out var asyncOperationHandle))
            {
                if (asyncOperationHandle.IsValid())
                {
                    var sceneInstance = asyncOperationHandle.Result;
                    await Addressables.UnloadSceneAsync(sceneInstance).Task;
                    m_loadingOperationHandles.Remove(sceneToUnload);
                }
            }
            else
            {
                var unloadAsyncOperationHandle = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(sceneToUnload);

                while (unloadAsyncOperationHandle.isDone == false)
                {
                    await Task.Yield();
                }
            }
                
            Resources.UnloadUnusedAssets();
            m_loadedScenes.Remove(scene);
        }
        
        private void RegisterLoadedScenes()
        {
            var loadedScenesCount = UnityEngine.SceneManagement.SceneManager.sceneCount;
            for (int i = 0; i < loadedScenesCount; i++)
            {
                var loadedScene = UnityEngine.SceneManagement.SceneManager.GetSceneAt(i);
                m_loadedScenes.Add(loadedScene);
            }
        }

        private bool SceneIsAlreadyLoaded(string sceneName)
        {
            for (int i = 0; i < m_loadedScenes.Count; i++)
            {
                if (m_loadedScenes[i].name == sceneName)
                {
                    return true;
                }
            }

            return false;
        }

#if QRCODE_TRC_CHECK
        private void CheckTrcSceneLoadingDuration(float duration)
        {
            if (duration < 30)
            {
                QRDebug.Debug(Constants.DebuggingChannels.TrcCheck, $"Scene Loading duration = {duration} < 30s.");
            }
            else
            {
                QRDebug.DebugError(Constants.DebuggingChannels.TrcCheck, $"Scene Loading duration = {duration} > 30s.");
            }
        }
#endif
    }

    public class SceneManagementException : Exception
    {
        public SceneManagementException(string message) : base(message)
        {
            
        }
    }
}
