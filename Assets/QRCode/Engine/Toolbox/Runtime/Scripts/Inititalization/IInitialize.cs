namespace QRCode.Engine.Toolbox.Initialization
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IInitialize
    {
        public Task InitAsync(CancellationToken cancellationToken);
    }
}