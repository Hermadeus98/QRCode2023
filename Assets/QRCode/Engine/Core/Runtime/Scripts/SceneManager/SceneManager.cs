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
    
    using QRCode.Engine.Core.Manager;
    using QRCode.Engine.Debugging;
    using QRCode.Engine.Core.SceneManagement.GeneratedEnum;
    using QRCode.Engine.Core.Tags;
    using QRCode.Engine.Toolbox.Database;
    using QRCode.Engine.Toolbox.Database.GeneratedEnums;

    public class SceneManager : GenericManagerBase<SceneManager>
    {
        #region Fields
        private Dictionary<string, AsyncOperationHandle<SceneInstance>> m_loadingOperationHandles = null;
        private List<Scene> m_loadedScenes = null;
        private SceneDatabase m_sceneDatabase = null;
        #endregion Fields

        #region Methods
        #region LifeCycle
        protected override Task InitAsync(CancellationToken cancellationToken)
        {
            m_loadingOperationHandles = new Dictionary<string, AsyncOperationHandle<SceneInstance>>();
            m_loadedScenes = new List<Scene>();
            m_sceneDatabase = DB.Instance.GetDatabase<SceneDatabase>(DBEnum.DB_Scenes);

            RegisterLoadedScenes();

            return Task.CompletedTask;
        }
        
        public override void Delete()
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

            if (m_loadedScenes != null)
            {
                m_loadedScenes.Clear();
                m_loadedScenes = null;
            }

            m_sceneDatabase = null;
            
            base.Delete();
        }
        #endregion LifeCycle

        #region Public Methods
        /// <summary>
        /// This class allows to load a scene asynchronously.
        /// </summary>
        public async Task LoadScene(DB_ScenesEnum sceneToLoad, IProgress<SceneLoadingInfo> sceneLoadingProgress, LoadSceneMode loadSceneMode = LoadSceneMode.Additive)
        {
            var sceneName = sceneToLoad.ToString();
            if (m_sceneDatabase.TryGetInDatabase(sceneName, out var sceneReference))
            {
                await LoadSceneInternal(sceneName, sceneReference,sceneLoadingProgress, loadSceneMode);
            }
            else
            {
                QRLogger.DebugError<CoreTags.SceneManagement>($"Cannot find {sceneToLoad.ToString()} in {m_sceneDatabase.name}", gameObject);
            }
        }
        
        /// <summary>
        /// This class allows to unload a scene asynchronously.
        /// </summary>
        public async Task UnLoadScene(DB_ScenesEnum sceneToUnload)
        {
            var sceneName = sceneToUnload.ToString();
            if (m_sceneDatabase.TryGetInDatabase(sceneName, out _))
            {
                await UnloadSceneInternal(sceneName);
            }
            else
            {
                QRLogger.DebugError<CoreTags.SceneManagement>($"Cannot find {sceneToUnload.ToString()} in {m_sceneDatabase.name}", gameObject);
            }
        }
        #endregion Public Methods

        #region Private Methods
        private async Task LoadSceneInternal(string sceneToLoad, SceneReference sceneReference, [NotNull] IProgress<SceneLoadingInfo> sceneLoadingProgress, LoadSceneMode loadSceneMode = LoadSceneMode.Additive)
        {
            if (SceneIsAlreadyLoaded(sceneToLoad))
            {
                QRLogger.DebugError<CoreTags.SceneManagement>($"The scene {sceneToLoad} is already loaded.");
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
                QRLogger.Debug<CoreTags.SceneManagement>($"Scene Loading duration = {duration} < 30s.");
            }
            else
            {
                QRLogger.DebugError<CoreTags.SceneManagement>($"Scene Loading duration = {duration} > 30s.");
            }
        }
#endif
        #endregion Private Methods
        #endregion Methods
    }
}
