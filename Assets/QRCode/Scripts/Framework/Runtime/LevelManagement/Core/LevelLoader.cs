namespace QRCode.Framework
{
    using System;
    using System.Threading.Tasks;
    using UnityEngine;

    [Serializable]
    public class LevelLoader
    {
        [SerializeField] private DB_LevelsEnum m_levelToLoad = DB_LevelsEnum.Undefined;
        [SerializeField] private DB_LoadingScreenEnum m_loadingScreenEnum = DB_LoadingScreenEnum.Undefined;
        [SerializeField] private bool m_forceReload = false;

        private ILevelLoadingManagementService m_levelLoadingManagementService;
        private ILevelLoadingManagementService LevelLoadingManagementService
        {
            get
            {
                if (m_levelLoadingManagementService == null)
                {
                    m_levelLoadingManagementService = ServiceLocator.Current.Get<ILevelLoadingManagementService>();
                }

                return m_levelLoadingManagementService;
            }
        }

        public async Task ChangeLevel()
        {
            await LevelLoadingManagementService.ChangeLevel(m_levelToLoad, m_loadingScreenEnum, m_forceReload);
        }
        
        public async Task LoadLevel()
        {
            await LevelLoadingManagementService.LoadLevel(m_levelToLoad, m_loadingScreenEnum, m_forceReload);
        }

        public async Task UnloadLevel()
        {
            await LevelLoadingManagementService.UnloadLevel(m_levelToLoad);
        }
    }
}
