namespace QRCode.Framework.Game
{
    using System.Threading.Tasks;
    using SceneManagement;
    using Sirenix.OdinInspector;
    using UnityEngine;

    public class Bootstrap : SerializedMonoBehaviour
    {
        [SerializeField] private DB_SceneEnum m_sceneUI = DB_SceneEnum.Scenes_UI;
        
        private void Awake()
        {
            Initialize();
        }

        private async void Initialize()
        {
            await SceneManager.Instance.LoadSceneGroup(m_sceneUI);
            
            InitializeGameStates();
            await PrepareSave();
            InitializeManagers();
            ExitBootstrapAndLaunchGame();
        }

        private void InitializeGameStates()
        {
            Game.PreInitialize();
        }

        private Task PrepareSave()
        {
            return Task.CompletedTask;
        }

        private void InitializeManagers()
        {
            
        }

        private void ExitBootstrapAndLaunchGame()
        {
            
        }
    }
}
