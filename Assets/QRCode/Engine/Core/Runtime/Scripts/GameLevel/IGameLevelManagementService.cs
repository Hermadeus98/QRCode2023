namespace QRCode.Engine.Core.GameLevels
{
    using System;
    using System.Threading.Tasks;
    using GeneratedEnums;
    using Toolbox.Pattern.Service;
    using UI.LoadingScreen.GeneratedEnums;

    public interface IGameLevelManagementService : IService
    {
        public Task ChangeLevel(DB_GameLevelsEnum gameLevelToLoad,
            DB_LoadingScreenEnum loadingScreenEnum, 
            bool forceReload = false, 
            bool activateOnLoad = true,
            int priority = 100);

        public Task UnloadCurrentLevel();

        public bool IsLoading();

        public event Func<Task> StartToLoadLevelAsync;
        public event Action StartToLoadLevel;
        public event Action<SceneLoadingInfo> LoadingLevel;
        public event Action FinishToLoadLevel;
        public event Func<Task> FinishToLoadLevelAsync;
    }
}
