namespace QRCode.Engine.Core.GameMode
{
    using System.Threading.Tasks;

    public interface IGameMode
    {
        public Task ConstructGameMode(params IGameModeModule[] gameModeModules);

        public T GetGameModeModule<T>() where T : IGameModeModule;
    }
}