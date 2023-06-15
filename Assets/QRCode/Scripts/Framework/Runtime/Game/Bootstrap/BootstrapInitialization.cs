namespace QRCode.Framework
{
    using System.Threading.Tasks;
    using Game;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class BootstrapInitialization : MonoBehaviour
    {
        [TitleGroup(K.InspectorGroups.References)] [SerializeField]
        private DB_ScenesEnum m_firstScene;

        private void Awake()
        {
            LaunchGame();
        }

        private async void LaunchGame()
        {
            while (Bootstrap.IsInit() == false)
            {
                await Task.Yield();
            }

            var sceneManagementService = ServiceLocator.Current.Get<ISceneManagementService>();
            await sceneManagementService.LoadScene(m_firstScene, LoadSceneMode.Single);
        }
    }
}
