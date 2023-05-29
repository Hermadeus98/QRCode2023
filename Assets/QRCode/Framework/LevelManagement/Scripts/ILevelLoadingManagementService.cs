namespace QRCode.Framework
{
    using System;
    using System.Threading.Tasks;
    using SceneManagement;

    public interface ILevelLoadingManagementService : IService
    {
        public Task<SceneLoadingInfo> ChangeLevel(DB_LevelsEnum levelToLoad,
            DB_LoadingScreenEnum loadingScreenEnum, 
            bool forceReload = false, 
            bool activateOnLoad = true,
            int priority = 100);
        
        public Task<SceneLoadingInfo> LoadLevel(DB_LevelsEnum levelToLoad,
            DB_LoadingScreenEnum loadingScreenEnum, 
            bool forceReload = false, 
            bool activateOnLoad = true,
            int priority = 100);

        public Task UnloadLevel(DB_LevelsEnum levelToUnload);

        public bool IsLoading();

        public event Func<Task> OnStartToLoadLevelAsync;
        public event Action OnStartToLoadLevel;
        public event Action<SceneLoadingInfo> OnLoadingLevel;
        public event Action OnFinishToLoadLevel;
        public event Func<Task> OnFinishToLoadLevelAsync;
    }
}
