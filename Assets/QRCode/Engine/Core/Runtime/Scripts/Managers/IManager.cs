namespace QRCode.Engine.Core.Managers
{
    using System.Threading;
    using System.Threading.Tasks;
    using Toolbox.Initialization;

    public interface IManager : IInitialize
    {
        public new Task InitAsync(CancellationToken cancellationToken);
    }
}