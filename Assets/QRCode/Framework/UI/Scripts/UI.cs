namespace QRCode.Framework
{
    public static class UI
    {
        private static Database<CanvasEnum, UICanvas> m_canvasDatabase = new Database<CanvasEnum, UICanvas>();

        private static Database<DB_LoadingScreenEnum, ILoadingScreen> m_loadedLoadingScreen =
            new Database<DB_LoadingScreenEnum, ILoadingScreen>();

        private static LoadingScreenDatabase m_loadingScreenDatabase = null;
        private static LoadingScreenDatabase LoadingScreenDatabase
        {
            get
            {
                if (m_loadingScreenDatabase == null)
                {
                    DB.Instance.TryGetDatabase<LoadingScreenDatabase>(DBEnum.DB_LoadingScreen,
                        out m_loadingScreenDatabase);
                }

                return m_loadingScreenDatabase;
            }
        }
        
        public static Database<CanvasEnum, UICanvas> GetCanvasDatabase
        {
            get
            {
                return m_canvasDatabase;
            }
        }

        public static ILoadingScreen GetLoadingScreen(DB_LoadingScreenEnum loadingScreenEnum)
        {
            LoadingScreenDatabase.TryGetInDatabase(loadingScreenEnum.ToString(), out var loadingScreen);

            if (m_loadedLoadingScreen.GetDatabase.ContainsKey(loadingScreenEnum))
            {
                m_loadedLoadingScreen.TryGetInDatabase(loadingScreenEnum, out var foundedLoadingScreen);
                return foundedLoadingScreen;
            }
            else
            {
                var loadingScreenInstance = LoadingScreenFactory.InstantiateLoadingScreen<ILoadingScreen>(loadingScreen);
                m_loadedLoadingScreen.AddToDatabase(loadingScreenEnum, loadingScreenInstance);
                return loadingScreenInstance;
            }
        }
    }
}
