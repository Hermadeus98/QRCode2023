namespace QRCode.Engine.Core.Player
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    
    using Managers;
    using Toolbox.Pattern.Singleton;

    public class PlayersManager : MonoBehaviourSingleton<PlayersManager>, IManager
    {
        private List<Player> m_players = null;
        
        public Task InitAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}