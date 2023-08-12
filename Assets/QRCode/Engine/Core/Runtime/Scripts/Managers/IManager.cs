namespace QRCode.Engine.Core.Managers
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IManager
    {
        public Task InitAsync(CancellationToken cancellationToken);
    }
}