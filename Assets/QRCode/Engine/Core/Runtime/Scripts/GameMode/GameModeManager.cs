namespace QRCode.Engine.Core.GameMode
{
    using System.Threading;
    using System.Threading.Tasks;
    
    using QRCode.Engine.Core.Manager;
    using QRCode.Engine.Toolbox.Optimization;

    public class GameModeManager : GenericManagerBase<GameModeManager>, IDeletable
    {
        private GameModeBase m_currentGameMode = null;

        protected override Task InitAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public bool TryGetCurrentGameMode<T>(out T gameMode) where T : GameModeBase
        {
            if (m_currentGameMode == null)
            {
                gameMode = null;
                return false;
            }
            else
            {
                gameMode = m_currentGameMode as T;
                return true;
            }
        }

        public void Delete()
        {
            
        }
    }
}