namespace QRCode.Engine.Core.GameLevels
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IGameLevelLoadable
    {
        public GameLevelLoadProgressionInfos GameLevelLoadProgressionInfos { get; set; }
        public Task Load(CancellationToken cancellationToken, Action onLoading, IProgress<float> progress);
    }
}
