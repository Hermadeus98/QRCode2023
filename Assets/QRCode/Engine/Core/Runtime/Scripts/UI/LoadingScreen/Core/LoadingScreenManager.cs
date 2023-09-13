namespace QRCode.Engine
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using QRCode.Engine.Core.Manager;
    using QRCode.Engine.Core.UI;
    using QRCode.Engine.Core.UI.LoadingScreen;
    using QRCode.Engine.Core.UI.LoadingScreen.GeneratedEnums;
    using QRCode.Engine.Toolbox.Database;
    using QRCode.Engine.Toolbox.Database.GeneratedEnums;
    using UnityEngine;

    public class LoadingScreenManager : GenericManagerBase<LoadingScreenManager>
    {
        private LoadingScreenDatabase _loadingScreenDatabase = null;
        private CanvasManager _canvasManager = null;
        private List<LoadingScreenHandle> _allLoadingScreenHandles = null;

        protected override Task InitAsync(CancellationToken cancellationToken)
        {
            _loadingScreenDatabase = DB.Instance.GetDatabase<LoadingScreenDatabase>(DBEnum.DB_LoadingScreen);
            _canvasManager = CanvasManager.Instance;
            _allLoadingScreenHandles = new List<LoadingScreenHandle>();
            
            return Task.CompletedTask;
        }

        public override void Delete()
        {
            base.Delete();
        }

        public async Task<LoadingScreenHandle> ShowLoadingScreen(DB_LoadingScreenEnum loadingScreenEnum)
        {
            var loadingScreenCanvas = _canvasManager.GetCanvas(CanvasEnum.GameCanvas);
            var loadingScreen = await CreateLoadingScreen(loadingScreenEnum, loadingScreenCanvas);
            await loadingScreen.Show();

            var loadingScreenHandle = new LoadingScreenHandle(loadingScreen);
            _allLoadingScreenHandles.Add(loadingScreenHandle);
            return loadingScreenHandle;
        }

        public async Task HideLoadingScreen(LoadingScreenHandle loadingScreenHandle)
        {
            var loadingScreenHandlesCount = _allLoadingScreenHandles.Count;
            for (int i = 0; i < loadingScreenHandlesCount; i++)
            {
                if (_allLoadingScreenHandles[i] == loadingScreenHandle)
                {
                    var handle = _allLoadingScreenHandles[i];
                    var loadingScreen = handle.LoadingScreen;
                    var hide = loadingScreen.Hide();
                    await hide;
                    return;
                }
            }
        }

        private async Task<ILoadingScreen> CreateLoadingScreen(DB_LoadingScreenEnum loadingScreenEnum, UICanvas canvasParent)
        {
            if (_loadingScreenDatabase.TryGetInDatabase(loadingScreenEnum.ToString(), out var loadingScreenReference))
            {
                var assetReference = loadingScreenReference.LoadingScreenAssetReference;
                var instantiateTask = assetReference.InstantiateAsync(transform.position, Quaternion.identity, canvasParent.transform).Task;
                var instance = await instantiateTask;
                var loadingScreenInstance = instance.GetComponent<ILoadingScreen>();
                return loadingScreenInstance;
            }

            return null;
        }
    }

    public class LoadingScreenHandle
    {
        private ILoadingScreen _loadingScreen = null;

        public ILoadingScreen LoadingScreen { get { return _loadingScreen; } }
        
        public LoadingScreenHandle(ILoadingScreen loadingScreen)
        {
            _loadingScreen = loadingScreen;
        }
    }
}
