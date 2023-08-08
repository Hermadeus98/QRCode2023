namespace QRCode.Engine.Core
{
    using System.Threading.Tasks;
    using Framework.Debugging;
    using UnityEngine;

    /// <summary>
    /// <see cref="GameInstance"/> represents the entry point of the game logic.
    /// </summary>
    public sealed class GameInstance
    {
        private static GameInstance Instance;
        public static GameInstance Get
        {
            get
            {
                return Instance;
            }
        }

        private GameInstanceInitializationDataComponent m_gameInstanceInitializationDataComponent = null;
        private bool m_managersAreInit = false;

        public bool ManagersAreInit
        {
            get
            {
                return m_managersAreInit;
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetGameInstance()
        {
            Instance = null;
        }
        
        public GameInstance(GameInstanceInitializationDataComponent gameInstanceInitializationData)
        {
            if (Instance != null)
            {
                QRDebug.DebugFatal(Constants.EngineConstants.EngineLogChannels.EngineChannel, $"A GameInstance is already existing in current context.");
                return;
            }

            Instance = this;
            
            m_gameInstanceInitializationDataComponent = gameInstanceInitializationData;
            
            LoadGame();
        }
        
        private async void LoadGame()
        {
            await InitManagers();
        }

        private async Task InitManagers()
        {
            var managersForGameInstanceInitialization = m_gameInstanceInitializationDataComponent.AllManagersForGameInstanceInitialization;
            var managersLength = managersForGameInstanceInitialization.Length;
            for (var i = 0; i < managersLength; i++)
            {
                await managersForGameInstanceInitialization[i].InitAsync();
            }
            m_managersAreInit = true;
        }
    }
}
