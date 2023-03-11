namespace QRCode.Framework
{
    using System;
    using System.Threading.Tasks;
    using UnityEngine;

    [Serializable]
    public class SceneLoader
    {
        [SerializeField] private DB_SceneEnum m_sceneToLoad = DB_SceneEnum.Undefined;
        [SerializeField] private DB_LoadingScreenEnum m_loadingScreenEnum = DB_LoadingScreenEnum.Undefined;
        [SerializeField] private bool m_forceReload = false;

        private ISceneManagementService m_sceneManagementService;
        private ISceneManagementService SceneManagementService
        {
            get
            {
                if (m_sceneManagementService == null)
                {
                    m_sceneManagementService = ServiceLocator.Current.Get<ISceneManagementService>();
                }

                return m_sceneManagementService;
            }
        }
        
        public async Task LoadScenes()
        {
            await SceneManagementService.LoadSceneGroup(m_sceneToLoad, m_loadingScreenEnum, m_forceReload);
        }

        public async Task UnloadScenes()
        {
            await SceneManagementService.UnloadSceneGroup(m_sceneToLoad);
        }
    }
}
