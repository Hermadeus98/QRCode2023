namespace QRCode.Engine.Core.GameMode
{
    using System.Threading.Tasks;

    public abstract class GameModeModuleBase : IGameModeModule
    {
        public abstract void OnConstruct(GameModeBase gameMode);
        
        public abstract Task InitAsync(GameModeBase gameMode);
    }
}
