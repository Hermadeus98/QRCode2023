namespace QRCode.Framework.SceneManagement.Tests
{
    using System.Threading.Tasks;
    using Sirenix.OdinInspector;
    using UnityEngine;

    public class SceneLoadingTest : SerializedMonoBehaviour
    {
        [SerializeField] private SceneReferenceGroupEnum m_groupToLoad;
        [SerializeField] private BlackScreen m_blackScreen;

        private Task<SceneLoadingInfo> task;

        [Button]
        private async void Load()
        {
            SceneManager.OnStartToLoadAsync += m_blackScreen.Show;
            SceneManager.OnFinishToLoadAsync += m_blackScreen.Hide;
            SceneManager.OnLoading += delegate(SceneLoadingInfo info)
            {
                m_blackScreen.Progress(info.GlobalProgress);
            };
            
            await SceneManager.Instance.LoadSceneGroup(m_groupToLoad);
        }

        [Button]
        private async void Unload()
        {
            await SceneManager.Instance.UnloadSceneGroup(m_groupToLoad);
        }
    }
}
