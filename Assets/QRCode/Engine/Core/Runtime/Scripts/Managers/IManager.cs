namespace QRCode.Engine.Core.Managers
{
    using System.Threading;
    using System.Threading.Tasks;
    
    using Engine.Toolbox.Optimization;

    public interface IManager : IDeletable
    {
        public Task InitAsync(CancellationToken cancellationToken);
    }
}