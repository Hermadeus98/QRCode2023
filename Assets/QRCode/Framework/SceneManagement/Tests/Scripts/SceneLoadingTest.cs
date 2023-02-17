namespace QRCode.Framework.SceneManagement.Tests
{
    using System.Threading.Tasks;
    using Sirenix.OdinInspector;
    using UnityEngine;

    public class SceneLoadingTest : SerializedMonoBehaviour
    {
        [SerializeField] private DB_SceneEnum m_groupToLoad;
        [SerializeField] private DB_LoadingScreenEnum m_loadingScreenEnum;

        private Task<SceneLoadingInfo> task;

        [Button]
        private async void Load()
        {
            var loadingScreen = UI.GetLoadingScreen(m_loadingScreenEnum);
            
            SceneManager.OnStartToLoadAsync += loadingScreen.Show;
            SceneManager.OnFinishToLoadAsync += loadingScreen.Hide;
            SceneManager.OnLoading += delegate(SceneLoadingInfo info)
            {
                loadingScreen.Progress(info);
            };
            
            //await SceneManager.Instance.LoadSceneGroup(m_groupToLoad);
        }

        [Button]
        private async void Unload()
        {
            //await SceneManager.Instance.UnloadSceneGroup(m_groupToLoad);
        }
    }
}
