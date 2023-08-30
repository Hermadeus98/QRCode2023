namespace QRCode.Engine.Core.GameLevels
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IGameLevelLoadable
    {
        public GameLevelLoadingInfo GameLevelLoadingInfo { get; set; }
        public Task Load(CancellationToken cancellationToken, Action onLoading, IProgress<float> progress);
    }
}
