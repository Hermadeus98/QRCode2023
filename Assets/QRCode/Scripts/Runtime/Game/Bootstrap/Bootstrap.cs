namespace QRCode.Framework.Game
{
    using System.Threading.Tasks;
    using Debugging;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using K = Framework.K;

    public static class Bootstrap
    {
        private static bool m_isInit = false;
        
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
            m_isInit = false;
            EnableLogger();

            ServiceLocator.Create();
            GameInstance.Create();
            
            RegisterServices();
            await PrepareSave();
            await PrepareUserSettings();
            InitializeGameStates();
            ExitBootstrapAndLaunchGame();

            await InitScenes();

            m_isInit = true;
            QRDebug.Debug(K.DebuggingChannels.LifeCycle, $"Bootstrapper has been initialized.");
        }
        
        private static void InitializeGameStates()
        {
            GameInstance.Instance.PreInitialize();
        }

        private static async Task PrepareSave()
        {
            var instantiateSaveServiceTask = ServiceSettings.SaveService.InstantiateAsync();
            var saveServiceInstance = instantiateSaveServiceTask.WaitForCompletion();
            var saveService = saveServiceInstance.GetComponent<ISaveService>();
            ServiceLocator.Current.RegisterService<ISaveService>(saveService);
            Object.DontDestroyOnLoad((Object)saveService);
            saveServiceInstance.AddComponent<ReleaseAddressableInstanceOnDestroy>();
            
            await saveService.Initialize();
        }

        private static async Task PrepareUserSettings()
        {
            IUserSettingsService userSettingsService = new UserSettingsService();
            ServiceLocator.Current.RegisterService<IUserSettingsService>(userSettingsService);
            await userSettingsService.Initialize();
        }
        
        private static void RegisterServices()
        {
            CreateSceneManagementService();
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
            sceneManagementServiceInstance.AddComponent<ReleaseAddressableInstanceOnDestroy>();
        }

        private static void CreateInputManagementService()
        {
            var instantiateInputManagementServiceTask = ServiceSettings.InputManagementService.InstantiateAsync();
            var inputManagementServiceInstance = instantiateInputManagementServiceTask.WaitForCompletion();
            var inputManagementService = inputManagementServiceInstance.GetComponent<IInputManagementService>();
            ServiceLocator.Current.RegisterService<IInputManagementService>(inputManagementService);
            Object.DontDestroyOnLoad((Object)inputManagementService);
            inputManagementServiceInstance.AddComponent<ReleaseAddressableInstanceOnDestroy>();
        }

        private static void CreateSceneManagementService()
        {
            var instantiateSceneManagementServiceTask = ServiceSettings.SceneManagementService.InstantiateAsync();
            var sceneManagementServiceInstance = instantiateSceneManagementServiceTask.WaitForCompletion();
            var sceneManagementService = sceneManagementServiceInstance.GetComponent<ISceneManagementService>();
            ServiceLocator.Current.RegisterService<ISceneManagementService>(sceneManagementService);
            Object.DontDestroyOnLoad((Object)sceneManagementService);
            sceneManagementServiceInstance.AddComponent<ReleaseAddressableInstanceOnDestroy>();
        }
        
        private static void CreateAudioService()
        {
            IAudioService audioService = new AudioService();
            ServiceLocator.Current.RegisterService<IAudioService>(audioService);
        }

        private static async Task InitScenes()
        {
#if DONT_USE_BOOTSTRAP_DEBUG
            var sceneManagementService = ServiceLocator.Current.Get<ISceneManagementService>();

            await sceneManagementService.LoadScene(DB_ScenesEnum.Scene_Main, LoadSceneMode.Additive);
            await sceneManagementService.LoadScene(DB_ScenesEnum.Scene_UI, LoadSceneMode.Additive);
#else
            var sceneManagementService = ServiceLocator.Current.Get<ISceneManagementService>();

            await sceneManagementService.LoadScene(DB_ScenesEnum.Scene_Main, LoadSceneMode.Single);
            await sceneManagementService.LoadScene(DB_ScenesEnum.Scene_UI, LoadSceneMode.Additive);
#endif
        }

        private static void ExitBootstrapAndLaunchGame()
        {
            
        }

        private static void EnableLogger()
        {
#if RELEASE
            Debug.unityLogger.logEnabled = false;
#endif
        }

        public static bool IsInit()
        {
            return m_isInit;
        }
    }
}
