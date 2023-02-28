namespace QRCode.Framework
{
    using System;
    using System.Threading.Tasks;
    using SceneManagement;

    public interface ISceneManagementService : IService
    {
        public Task<SceneLoadingInfo> LoadSceneGroup(DB_SceneEnum sceneReferenceGroupToLoad,
            DB_LoadingScreenEnum loadingScreenEnum, bool forceReload = false, bool activateOnLoad = true,
            int priority = 100);

        public Task UnloadSceneGroup(DB_SceneEnum sceneReferenceGroupToUnload);

        public event Func<Task> OnStartToLoadAsync;
        public event Action OnStartToLoad;
        public event Action<SceneLoadingInfo> OnLoading;
        public event Action OnFinishToLoad;
        public event Func<Task> OnFinishToLoadAsync;
    }
}
