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

            RegisterServices();
            await PrepareSave();
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
            CreateSceneManagementService();
            CreateAudioService();
        }

        private static void CreateSceneManagementService()
        {
            ISceneManagementService sceneManagementService = Object.Instantiate((SceneManager)ServiceSettings.SceneManagementService);
            ServiceLocator.Current.RegisterService<ISceneManagementService>(sceneManagementService);
            Object.DontDestroyOnLoad((Object)sceneManagementService);
            
            if (SaveServiceSettings.Instance.SaveAsyncBeforeSceneLoading)
            {
                sceneManagementService.OnStartToLoadAsync += async delegate
                {
                    var saveService = ServiceLocator.Current.Get<ISaveService>();
                    await saveService.SaveGame();
                };
            }

            if (SaveServiceSettings.Instance.LoadAsyncAfterSceneLoading)
            {
                sceneManagementService.OnFinishToLoadAsync += delegate
                {
                    Load.Current.LoadObjects();
                    return Task.CompletedTask;
                };
            }
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
