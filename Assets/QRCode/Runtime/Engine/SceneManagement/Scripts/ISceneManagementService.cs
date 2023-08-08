namespace QRCode.Framework
{
    using System.Threading.Tasks;
    using UnityEngine.SceneManagement;

    public interface ISceneManagementService : IService
    {
        public Task LoadScene(DB_ScenesEnum sceneToLoad, LoadSceneMode loadSceneMode);
        public Task UnLoadScene(DB_ScenesEnum sceneToUnload);
    }
}
