namespace QRCode.Engine.Core.GameLevels
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IGameLevel
    {
        public void BuildGameLevelModules();

        public Task LoadLevel(CancellationToken cancellationToken, IProgress<GameLevelLoadingInfo> progress);
    }
}