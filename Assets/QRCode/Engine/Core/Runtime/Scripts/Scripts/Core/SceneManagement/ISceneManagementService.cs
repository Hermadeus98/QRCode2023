namespace QRCode.Engine.Core.SceneManagement
{
    using System.Threading.Tasks;
    using GeneratedEnum;
    using Toolbox.Pattern.Service;
    using UnityEngine.SceneManagement;

    public interface ISceneManagementService : IService
    {
        public Task LoadScene(DB_ScenesEnum sceneToLoad, LoadSceneMode loadSceneMode);
        public Task UnLoadScene(DB_ScenesEnum sceneToUnload);
    }
}
