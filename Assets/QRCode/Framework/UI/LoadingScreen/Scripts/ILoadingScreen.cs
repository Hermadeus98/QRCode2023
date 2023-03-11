namespace QRCode.Framework
{
    using System.Threading.Tasks;
    using SceneManagement;

    public interface ILoadingScreen
    {
        public Task Show();
        public Task Hide();

        public void Progress(SceneLoadingInfo loadingInfo);
    }
}
