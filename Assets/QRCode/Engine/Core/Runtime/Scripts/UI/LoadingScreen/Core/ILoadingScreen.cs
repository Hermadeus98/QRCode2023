namespace QRCode.Engine.Core.UI.LoadingScreen
{
    using System.Threading.Tasks;
    using GameLevels;

    public interface ILoadingScreen
    {
        public Task Show();
        public Task Hide();

        public void Progress(float progress, string progressInfo);
    }
}
