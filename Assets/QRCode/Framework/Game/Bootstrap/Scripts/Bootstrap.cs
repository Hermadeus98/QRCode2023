namespace QRCode.Framework.Game
{
    using System.Threading.Tasks;
    using SceneManagement;
    using UnityEngine;

    public static class Bootstrap
    {
        private static ServiceSettings m_serviceSettings = null;
        private static ServiceSettings ServiceSettings
        {
            get
            {
                if (m_serviceSettings == null)
                {
                    m_serviceSettings = ServiceSettings.Instance;
                }

                return m_serviceSettings;
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static async void Initialize()
        {
            ServiceLocator.Create();

            await PrepareSave();
            RegisterServices();
            InitializeGameStates();
            ExitBootstrapAndLaunchGame();
        }

        private static void InitializeGameStates()
        {
            Game.PreInitialize();
        }

        private static async Task PrepareSave()
        {
            var gameObject = new GameObject("[SERVICE] Save System");
            ISaveService saveService = gameObject.AddComponent<SaveService>();
            ServiceLocator.Current.RegisterService<ISaveService>(saveService);
            await saveService.Initialize();
        }

        private static void RegisterServices()
        {
            //SceneManagementService
            ISceneManagementService sceneManagementService = Object.Instantiate((SceneManager)ServiceSettings.SceneManagementService);
            ServiceLocator.Current.RegisterService<ISceneManagementService>(sceneManagementService);
            Object.DontDestroyOnLoad(sceneManagementService as Object);
            
        }

        private static void ExitBootstrapAndLaunchGame()
        {
            
        }
    }
}
