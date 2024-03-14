namespace QRCode.Engine.Core.GameMode
{
    using System.Threading;
    using System.Threading.Tasks;
    using QRCode.Engine.Core.Manager;
    using QRCode.Engine.Toolbox.Optimization;

    public class GameModeManager : GenericManagerBase<GameModeManager>, IDeletable
    {
        private AGameMode _currentGameMode = null;
        public AGameMode CurrentGameMode { get { return _currentGameMode; } }

        protected override Task InitAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public void SwitchGameMode<t_gameModeType>() where t_gameModeType : AGameMode, new()
        {
            if (_currentGameMode != null)
            {
                _currentGameMode.Delete();
            }

            _currentGameMode = new t_gameModeType();
            _currentGameMode.ConstructGameMode();
        }

        public override void Delete()
        {
            if (_currentGameMode != null)
            {
                _currentGameMode.Delete();
                _currentGameMode = null;
            }
        }
    }
}