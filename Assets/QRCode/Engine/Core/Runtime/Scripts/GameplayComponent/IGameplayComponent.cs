namespace QRCode.Engine.Core
{
    using QRCode.Engine.Core.GameLevels;
    using QRCode.Engine.Toolbox.Optimization;
    using PauseInfo = QRCode.Engine.Core.GameInstance.PauseInfo;

    /// <summary>
    /// A gameplay component implements some useful function called during the game flow.
    /// </summary>
    public interface IGameplayComponent : IDeletable
    {
        /// <summary>
        /// This function is call after a <see cref="AGameLevel"/> is loaded.
        /// </summary>
        public void OnLevelLoaded();

        /// <summary>
        /// This function is call before a <see cref="AGameLevel"/> starts to be unloaded.
        /// </summary>
        public void OnLevelUnloaded();

        /// <summary>
        /// This function is call when the <see cref="GameInstance"/> is setup correctly.
        /// </summary>
        public void OnGameInstanceIsReady();

        public void OnGameStart();

        public void OnGameUpdate();

        public void OnGameEnd();

        public void OnGameRestart();

        public void OnGamePause(PauseInfo pauseInfo);
    }
}
