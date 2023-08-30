namespace QRCode.Engine.Core.GameLevels
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Toolbox.Optimization;

    public interface IGameLevel : IDeletable
    {
        public void BuildGameLevelModules();

        public Task LoadLevel(CancellationToken cancellationToken, IProgress<GameLevelLoadingInfo> progress);
    }
}