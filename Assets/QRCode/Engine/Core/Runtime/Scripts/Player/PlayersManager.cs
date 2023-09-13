namespace QRCode.Engine.Core.Player
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using QRCode.Engine.Core.Manager;
    using QRCode.Engine.Toolbox.Optimization;

    public class PlayersManager : GenericManagerBase<PlayersManager>, IDeletable
    {
        private List<Player> m_players = null;
        
        protected override Task InitAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public override void Delete()
        {
            
        }
    }
}