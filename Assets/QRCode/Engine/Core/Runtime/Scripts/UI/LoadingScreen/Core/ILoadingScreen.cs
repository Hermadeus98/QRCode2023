namespace QRCode.Engine.Core.UI.LoadingScreen
{
    using System.Threading.Tasks;
    using GameLevel;

    public interface ILoadingScreen
    {
        public Task Show();
        public Task Hide();

        public void Progress(SceneLoadingInfo loadingInfo);
    }
}
