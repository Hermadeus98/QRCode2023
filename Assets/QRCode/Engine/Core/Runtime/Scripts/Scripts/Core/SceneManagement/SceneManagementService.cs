namespace QRCode.Engine.Core.SceneManagement
{
    using System;
    using System.Collections.Generic;
    using Toolbox;
    using System.Threading.Tasks;
    using Debugging;
    using Framework;
    using GeneratedEnum;
    using Toolbox.Database;
    using Toolbox.Database.GeneratedEnums;
    using UnityEngine;
    using UnityEngine.AddressableAssets;
    using UnityEngine.ResourceManagement.AsyncOperations;
    using UnityEngine.ResourceManagement.ResourceProviders;
    using UnityEngine.SceneManagement;

    public static class SceneManagementService
    {
        private static readonly Dictionary<DB_ScenesEnum, AsyncOperationHandle<SceneInstance>> m_scenesInstanceHandle = new();

        private static SceneDatabase m_sceneDatabase = null;
        private static SceneDatabase SceneDatabase
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

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Reset()
        {
            m_scenesInstanceHandle.Clear();
        }

        public static async Task LoadScene(DB_ScenesEnum sceneToLoad, LoadSceneMode loadSceneMode = LoadSceneMode.Additive)
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

        public static async Task UnLoadScene(DB_ScenesEnum sceneToUnload)
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

        private static bool SceneIsAlreadyLoad(SceneReference sceneReference)
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
