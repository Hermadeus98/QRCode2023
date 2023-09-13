namespace QRCode.Engine.Core.GameLevels
{
    using System;
    using System.Threading.Tasks;
    using GeneratedEnums;
    using UI.LoadingScreen.GeneratedEnums;
    using UnityEngine;

    [Serializable]
    public class GameLevelLoader
    {
        [SerializeField] private DB_GameLevelsEnum gameLevelToLoad = DB_GameLevelsEnum.Undefined;
        [SerializeField] private DB_LoadingScreenEnum m_loadingScreenEnum = DB_LoadingScreenEnum.Undefined;
        [SerializeField] private bool m_forceReload = false;

        private GameLevelManager m_gameLevelManager;
        private GameLevelManager GameLevelManager
        {
            get
            {
                if (m_gameLevelManager == null)
                {
                    m_gameLevelManager = GameLevelManager.Instance;
                }

                return m_gameLevelManager;
            }
        }

        public async Task ChangeLevel()
        {
            await GameLevelManager.ChangeLevel(gameLevelToLoad, m_loadingScreenEnum, m_forceReload);
        }
    }
}
