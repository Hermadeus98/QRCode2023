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
            await PrepareSave();
            RegisterServices();
            InitializeServices();
            InitializeGameStates();
            ExitBootstrapAndLaunchGame();
        }

        private static void InitializeGameStates()
        {
            Game.PreInitialize();
        }

        private static Task PrepareSave()
        {
            return Task.CompletedTask;
        }

        private static void RegisterServices()
        {
            ServiceLocator.Create();
            
            //SceneManagementService
            ISceneManagementService sceneManagementService = Object.Instantiate((SceneManager)ServiceSettings.SceneManagementService);
            ServiceLocator.RegisterService<ISceneManagementService>(sceneManagementService);
        }

        private static void InitializeServices()
        {
            ServiceLocator.InitializeService();
        }

        private static void ExitBootstrapAndLaunchGame()
        {
            
        }
    }
}
