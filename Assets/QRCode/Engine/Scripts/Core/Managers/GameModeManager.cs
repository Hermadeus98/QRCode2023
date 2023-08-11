namespace QRCode.Engine.Core.GameMode
{
    using System.Threading.Tasks;
    using Framework.Singleton;
    using Managers;

    public class GameModeManager : MonoBehaviourSingleton<GameModeManager>, IManager
    {
        private GameModeBase m_currentGameMode = null;
        
        public Task InitAsync()
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
    }
}