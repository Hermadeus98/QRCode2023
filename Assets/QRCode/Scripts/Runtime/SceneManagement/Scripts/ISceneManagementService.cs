namespace QRCode.Framework
{
    using System.Threading.Tasks;

    public interface ISceneManagementService : IService
    {
        public Task LoadScene(DB_ScenesEnum sceneToLoad);
        public Task UnLoadScene(DB_ScenesEnum sceneToUnload);
    }
}
