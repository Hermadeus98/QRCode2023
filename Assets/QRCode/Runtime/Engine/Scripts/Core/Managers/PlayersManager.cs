namespace QRCode.Engine.Core.Player
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Framework.Singleton;
    using Managers;

    public class PlayersManager : MonoBehaviourSingleton<PlayersManager>, IManager
    {
        private List<Player> m_players = null;
        
        public Task InitAsync()
        {
            return Task.CompletedTask;
        }
    }
}