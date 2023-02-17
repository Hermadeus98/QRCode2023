namespace QRCode.Framework
{
    using UnityEngine;

    public static class LoadingScreenFactory
    {
        public static ILoadingScreen InstantiateLoadingScreen<T>(ILoadingScreen loadingScreenPrefab)
        {
            UI.GetCanvasDatabase.TryGetInDatabase(CanvasEnum.LoadingScreenCanvas, out var uiCanvas);
            var loadingScreen = Object.Instantiate((Component)loadingScreenPrefab, uiCanvas.transform);
            return loadingScreen as ILoadingScreen;
        }
    }
}
