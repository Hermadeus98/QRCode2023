namespace QRCode.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Debugging;
    using Sirenix.OdinInspector;
    using UnityEngine.AddressableAssets;
    using UnityEngine.ResourceManagement.AsyncOperations;
    using UnityEngine.ResourceManagement.ResourceProviders;
    using UnityEngine.SceneManagement;

    public class SceneManagementService : SerializedMonoBehaviour, ISceneManagementService
    {
        private readonly Dictionary<DB_ScenesEnum, AsyncOperationHandle<SceneInstance>> m_scenesInstanceHandle = new();

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
                        QRDebug.DebugError(K.DebuggingChannels.SceneManager, $"Cannot load SceneDatabase, verify DB.", gameObject);
                    }
                }

                return m_sceneDatabase;
            }
        }

        private void OnDestroy()
        {
            m_scenesInstanceHandle.Clear();
        }

        public async Task LoadScene(DB_ScenesEnum sceneToLoad, LoadSceneMode loadSceneMode = LoadSceneMode.Additive)
        {
            if (SceneDatabase.TryGetInDatabase(sceneToLoad.ToString(), out var sceneReference))
            {
                if (SceneIsAlreadyLoad(sceneReference))
                {
                    return;
                }
                
                var loadOperationHandle = sceneReference.Scene.LoadSceneAsync(loadSceneMode);
                await loadOperationHandle.Task;
                
                m_scenesInstanceHandle.Add(sceneToLoad, loadOperationHandle);
            }
            else
            {
                throw new SceneManagementException($"Cannot find {sceneToLoad.ToString()} in {m_sceneDatabase.name}");
            }
        }

        public async Task UnLoadScene(DB_ScenesEnum sceneToUnload)
        {
            if (SceneDatabase.TryGetInDatabase(sceneToUnload.ToString(), out var sceneReference))
            {
                var unloadOperationHandle = sceneReference.Scene.UnLoadScene();
                await unloadOperationHandle.Task;
                
                Addressables.ReleaseInstance(m_scenesInstanceHandle[sceneToUnload]);
                m_scenesInstanceHandle.Remove(sceneToUnload);
            }
            else
            {
                throw new SceneManagementException($"Cannot find {sceneToUnload.ToString()} in {m_sceneDatabase.name}");
            }
        }

        private bool SceneIsAlreadyLoad(SceneReference sceneReference)
        {
            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                if (SceneManager.GetSceneAt(i).name == sceneReference.Scene.editorAsset.name)
                {
                    return true;
                }
            }

            return false;
        }
    }

    public class SceneManagementException : Exception
    {
        public SceneManagementException(string message) : base(message)
        {
            
        }
    }
}
