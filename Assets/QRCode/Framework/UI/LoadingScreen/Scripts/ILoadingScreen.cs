namespace QRCode.Framework
{
    using System.Threading.Tasks;

    public interface ILoadingScreen
    {
        public Task Show();
        public Task Hide();
        public void Progress(float progression);
    }
}
