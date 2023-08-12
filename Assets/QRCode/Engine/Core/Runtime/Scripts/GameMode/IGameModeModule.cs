namespace QRCode.Engine.Core.GameMode
{
    using System.Threading.Tasks;

    public interface IGameModeModule
    {
        public void OnConstruct(GameModeBase gameMode);
        
        public Task InitAsync(GameModeBase gameMode);
    }
}