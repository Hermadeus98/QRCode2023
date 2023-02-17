namespace QRCode.Framework.Game
{
    using Framework;
    using Framework.Debugging;

    public interface IGameplayObject
    {
        public void OnPreInitialize();
        public void OnGameStart();
        public void OnGamePause(PauseInfo pauseInfo);
    }
}
