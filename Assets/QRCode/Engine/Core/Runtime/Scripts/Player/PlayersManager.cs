namespace QRCode.Engine.Core.Player
{
    using System.Threading;
    using System.Threading.Tasks;
    using QRCode.Engine.Core.Manager;

    public class PlayersManager : GenericManagerBase<PlayersManager>
    {
        //private List<Player> m_players = null;
        
        protected override Task InitAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public override void Delete()
        {
            
        }
    }
}