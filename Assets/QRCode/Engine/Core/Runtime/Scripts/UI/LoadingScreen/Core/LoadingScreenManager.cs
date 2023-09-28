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
    using QRCode.Engine.Toolbox.Optimization;
    using UnityEngine;
    using UnityEngine.AddressableAssets;

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
            _loadingScreenDatabase = null;
            _canvasManager = null;

            if (_allLoadingScreenHandles != null)
            {
                _allLoadingScreenHandles.Clear();
                _allLoadingScreenHandles = null;
            }
            
            base.Delete();
        }

        public async Task<LoadingScreenHandle> GetLoadingScreen(DB_LoadingScreenEnum loadingScreenEnum)
        {
            UICanvas loadingScreenCanvas = _canvasManager.GetCanvas(CanvasEnum.LoadingScreenCanvas);
            ILoadingScreen loadingScreen = await CreateLoadingScreen(loadingScreenEnum, loadingScreenCanvas);

            LoadingScreenHandle loadingScreenHandle = new LoadingScreenHandle(loadingScreen);
            _allLoadingScreenHandles.Add(loadingScreenHandle);
            
            return loadingScreenHandle;
        }

        public async Task HideLoadingScreen(LoadingScreenHandle loadingScreenHandle)
        {
            int loadingScreenHandlesCount = _allLoadingScreenHandles.Count;
            for (int i = 0; i < loadingScreenHandlesCount; i++)
            {
                if (_allLoadingScreenHandles[i] == loadingScreenHandle)
                {
                    LoadingScreenHandle handle = _allLoadingScreenHandles[i];
                    ILoadingScreen loadingScreen = handle.LoadingScreen;
                    
                    await loadingScreen.Hide();
                    
                    _allLoadingScreenHandles.Remove(handle);
                    handle.Delete();
                    
                    return;
                }
            }
        }

        private async Task<ILoadingScreen> CreateLoadingScreen(DB_LoadingScreenEnum loadingScreenEnum, UICanvas canvasParent)
        {
            if (_loadingScreenDatabase.TryGetInDatabase(loadingScreenEnum.ToString(), out var loadingScreenReference))
            {
                AssetReference assetReference = loadingScreenReference.LoadingScreenAssetReference;
                Task<GameObject> instantiateTask = assetReference.InstantiateAsync(canvasParent.transform).Task;
                GameObject loadingScreenGameObject = await instantiateTask;
                loadingScreenGameObject.transform.localPosition = Vector3.zero;
                loadingScreenGameObject.transform.localRotation = Quaternion.identity;

                ILoadingScreen loadingScreenInstance = loadingScreenGameObject.GetComponent<ILoadingScreen>();
                return loadingScreenInstance;
            }

            return null;
        }
    }

    public class LoadingScreenHandle : IDeletable
    {
        private ILoadingScreen _loadingScreen = null;

        public ILoadingScreen LoadingScreen { get { return _loadingScreen; } }
        
        public LoadingScreenHandle(ILoadingScreen loadingScreen)
        {
            _loadingScreen = loadingScreen;
        }

        public void Delete()
        {
            if (_loadingScreen != null)
            {
                if (_loadingScreen is Component component)
                {
                    Object.Destroy(component.gameObject);
                }

                _loadingScreen = null;
            }
        }
    }
}
