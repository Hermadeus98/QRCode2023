namespace QRCode.Framework
{
    using System;
    using System.Threading.Tasks;
    using SceneManagement;
    using UnityEngine;

    [Serializable]
    public class GameLevelLoader
    {
        [SerializeField] private DB_GameLevelsEnum gameLevelToLoad = DB_GameLevelsEnum.Undefined;
        [SerializeField] private DB_LoadingScreenEnum m_loadingScreenEnum = DB_LoadingScreenEnum.Undefined;
        [SerializeField] private bool m_forceReload = false;

        private IGameLevelManagementService m_gameLevelManagementService;
        private IGameLevelManagementService GameLevelManagementService
        {
            get
            {
                if (m_gameLevelManagementService == null)
                {
                    m_gameLevelManagementService = GameLevelManager.Instance;
                }

                return m_gameLevelManagementService;
            }
        }

        public async Task ChangeLevel()
        {
            await GameLevelManagementService.ChangeLevel(gameLevelToLoad, m_loadingScreenEnum, m_forceReload);
        }
    }
}
