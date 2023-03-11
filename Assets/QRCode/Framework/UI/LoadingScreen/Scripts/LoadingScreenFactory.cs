namespace QRCode.Framework
{
    using System.Threading.Tasks;
    using UnityEngine.AddressableAssets;

    public static class LoadingScreenFactory
    {
        public static async Task<ILoadingScreen> InstantiateLoadingScreen<T>(AssetReference loadingScreenPrefab)
        {
            UI.CanvasDatabase.TryGetInDatabase(CanvasEnum.LoadingScreenCanvas, out var uiCanvas);
            var op = loadingScreenPrefab.InstantiateAsync(uiCanvas.transform);
            while (!op.IsDone)
            {
                await Task.Yield();
            }
            //var loadingScreen = Object.Instantiate(loadingScreenPrefab, uiCanvas.transform);

            var result = op.Result;
            var loadingScreen = result.GetComponent<ILoadingScreen>();
            return loadingScreen;
        }
    }
}
