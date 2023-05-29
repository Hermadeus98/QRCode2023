namespace QRCode.Framework.Game
{
    using System.Threading.Tasks;
    using Debugging;
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

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static async void Initialize()
        {
            ServiceLocator.Create();
            GameInstance.Create();

            RegisterServices();
            await PrepareSave();
            InitializeGameStates();
            ExitBootstrapAndLaunchGame();
            
            QRDebug.Debug(K.DebuggingChannels.LifeCycle, $"Bootstrapper has been initialized.");
        }
        
        private static void InitializeGameStates()
        {
            GameInstance.Instance.PreInitialize();
        }

        private static async Task PrepareSave()
        {
            var gameObject = new GameObject("[SERVICE] Save System");
            ISaveService saveService = gameObject.AddComponent<SaveService>();
            ServiceLocator.Current.RegisterService<ISaveService>(saveService);
            Object.DontDestroyOnLoad((Object)saveService);
            await saveService.Initialize();
        }

        private static void RegisterServices()
        {
            CreateLevelManagementService();
            CreateInputManagementService();
            CreateAudioService();
        }

        private static void CreateLevelManagementService()
        {
            var instantiateSceneManagementServiceTask = ServiceSettings.LevelLoadingManagementService.InstantiateAsync();
            var sceneManagementServiceInstance = instantiateSceneManagementServiceTask.WaitForCompletion();
            var sceneManagementService = sceneManagementServiceInstance.GetComponent<ILevelLoadingManagementService>();
            ServiceLocator.Current.RegisterService<ILevelLoadingManagementService>(sceneManagementService);
            Object.DontDestroyOnLoad((Object)sceneManagementService);
        }

        private static void CreateInputManagementService()
        {
            var instantiateInputManagementServiceTask = ServiceSettings.InputManagementService.InstantiateAsync();
            var inputManagementServiceInstance = instantiateInputManagementServiceTask.WaitForCompletion();
            var inputManagementService = inputManagementServiceInstance.GetComponent<IInputManagementService>();
            ServiceLocator.Current.RegisterService<IInputManagementService>(inputManagementService);
            Object.DontDestroyOnLoad((Object)inputManagementService);
        }

        private static void CreateAudioService()
        {
            IAudioService audioService = new AudioService();
            ServiceLocator.Current.RegisterService<IAudioService>(audioService);
        }

        private static void ExitBootstrapAndLaunchGame()
        {
            
        }
    }
}
