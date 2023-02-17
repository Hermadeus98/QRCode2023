namespace QRCode.Framework
{
    using System;
    using System.Threading.Tasks;
    using SceneManagement;
    using UnityEngine;

    [Serializable]
    public class SceneLoader
    {
        [SerializeField] private DB_SceneEnum m_sceneToLoad = DB_SceneEnum.Undefined;
        [SerializeField] private DB_LoadingScreenEnum m_loadingScreenEnum = DB_LoadingScreenEnum.Undefined;
        [SerializeField] private bool m_forceReload = false;
        
        private EventOneShot m_onBeforeLoad = null;
        private EventOneShot m_onAfterLoad = null;

        public EventOneShot OnBeforeLoad => m_onBeforeLoad;
        public EventOneShot OnAfterLoad => m_onAfterLoad;
        
        public async Task LoadScenes()
        {
            m_onBeforeLoad?.Invoke();
            await SceneManager.Instance.LoadSceneGroup(m_sceneToLoad, m_loadingScreenEnum, m_forceReload);
            m_onAfterLoad?.Invoke();
        }

        public async Task UnloadScenes()
        {
            await SceneManager.Instance.UnloadSceneGroup(m_sceneToLoad);
        }
    }
}
