namespace QRCode.Framework
{
    using System;
    using System.Threading.Tasks;
    using Debugging;
    using Sirenix.OdinInspector;
    using UnityEngine.SceneManagement;

    public class SceneManagementService : SerializedMonoBehaviour, ISceneManagementService
    {
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
        
        public async Task<SceneReference> LoadScene(DB_ScenesEnum sceneToLoad)
        {
            if (SceneDatabase.TryGetInDatabase(sceneToLoad.ToString(), out var sceneReference))
            {
                var op = sceneReference.Scene.LoadSceneAsync(LoadSceneMode.Additive).Task;
                await op;
                return sceneReference;
            }
            else
            {
                throw new SceneManagementException($"Cannot find {sceneToLoad.ToString()} in {m_sceneDatabase.name}");
            }
        }

        public async Task<SceneReference> UnLoadScene(DB_ScenesEnum sceneToUnload)
        {
            if (SceneDatabase.TryGetInDatabase(sceneToUnload.ToString(), out var sceneReference))
            {
                var op = sceneReference.Scene.UnLoadScene().Task;
                await op;
                return sceneReference;
            }
            else
            {
                throw new SceneManagementException($"Cannot find {sceneToUnload.ToString()} in {m_sceneDatabase.name}");
            }
        }
    }

    public class SceneManagementException : Exception
    {
        public SceneManagementException(string message) : base(message)
        {
            
        }
    }
}
