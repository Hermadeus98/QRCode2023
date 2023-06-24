namespace QRCode.Framework
{
    using System.Threading.Tasks;
    using UnityEngine;

    public static class UI
    {
        private static Database<CanvasEnum, UICanvas> m_canvasDatabase = null;
        private static Database<string, UIView> m_UIViewDatabase = null;
        private static Database<DB_LoadingScreenEnum, ILoadingScreen> m_loadedLoadingScreen = null;
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
        
        public static Database<CanvasEnum, UICanvas> CanvasDatabase
        {
            get
            {
                return m_canvasDatabase;
            }
        }

        public static Database<string, UIView> UIViewDatabase
        {
            get
            {
                return m_UIViewDatabase;
            }
        }


        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Init()
        {
            m_canvasDatabase = new Database<CanvasEnum, UICanvas>();
            m_loadedLoadingScreen = new Database<DB_LoadingScreenEnum, ILoadingScreen>();
            m_UIViewDatabase = new Database<string, UIView>();
        }

        public static async Task<ILoadingScreen> GetLoadingScreen(DB_LoadingScreenEnum loadingScreenEnum)
        {
            LoadingScreenDatabase.TryGetInDatabase(loadingScreenEnum.ToString(), out var loadingScreen);

            if (m_loadedLoadingScreen.GetDatabase.ContainsKey(loadingScreenEnum))
            {
                m_loadedLoadingScreen.TryGetInDatabase(loadingScreenEnum, out var foundedLoadingScreen);
                return foundedLoadingScreen;
            }
            else
            {
                var loadingScreenInstance = await LoadingScreenFactory.InstantiateLoadingScreen<ILoadingScreen>(loadingScreen.LoadingScreenAssetReference);
                m_loadedLoadingScreen.AddToDatabase(loadingScreenEnum, loadingScreenInstance);
                return loadingScreenInstance;
            }
        }
    }
}
