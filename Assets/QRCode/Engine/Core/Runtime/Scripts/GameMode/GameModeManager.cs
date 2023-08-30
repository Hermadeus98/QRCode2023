namespace QRCode.Engine.Core.GameMode
{
    using System.Threading;
    using System.Threading.Tasks;
    
    using Engine.Core.Managers;
    using Engine.Toolbox.Pattern.Singleton;

    public class GameModeManager : MonoBehaviourSingleton<GameModeManager>, IManager
    {
        private GameModeBase m_currentGameMode = null;
        
        public Task InitAsync(CancellationToken cancellationToken)
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