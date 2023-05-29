namespace QRCode.Framework
{
    using System.Threading.Tasks;

    public interface ISceneManagementService : IService
    {
        public Task<SceneReference> LoadScene(DB_ScenesEnum sceneToLoad);
        public Task<SceneReference> UnLoadScene(DB_ScenesEnum sceneToUnload);
    }
}
