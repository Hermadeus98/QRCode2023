namespace QRCode.Engine.Core
{
    using GameInstance;

    public interface IGameplayComponent
    {
        public virtual void OnLevelLoaded() { }

        public virtual void OnLevelUnloaded() { }

        public virtual void OnGameStart() { }

        public virtual void OnGameUpdate() { }

        public virtual void OnGameEnd() { }
        
        public virtual void OnGameRestart() { }

        public virtual void OnGamePause(PauseInfo pauseInfo) { }
    }
}
