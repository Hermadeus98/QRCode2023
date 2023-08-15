namespace QRCode.Engine.Core.GameLevels
{
    using System;
    using System.Threading.Tasks;
    using GeneratedEnums;
    using Toolbox.Pattern.Service;
    using UI.LoadingScreen.GeneratedEnums;

    public interface IGameLevelManagementService : IService
    {
        public Task<SceneLoadingInfo> ChangeLevel(DB_GameLevelsEnum gameLevelToLoad,
            DB_LoadingScreenEnum loadingScreenEnum, 
            bool forceReload = false, 
            bool activateOnLoad = true,
            int priority = 100);

        public Task UnloadCurrentLevel();

        public bool IsLoading();

        public event Func<Task> OnStartToLoadLevelAsync;
        public event Action OnStartToLoadLevel;
        public event Action<SceneLoadingInfo> OnLoadingLevel;
        public event Action OnFinishToLoadLevel;
        public event Func<Task> OnFinishToLoadLevelAsync;
    }
}
