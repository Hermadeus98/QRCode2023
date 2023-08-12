namespace QRCode.Engine.Core.Boot
{
    using GameInstance;
    using UnityEngine;
    using GameInstance = QRCode.Engine.Core.GameInstance.GameInstance;
    
    /// <summary>
    /// <see cref="Bootstrap"/> represents the first entry point of the application.
    /// This is the only one admitted entry point of all the application, it must be contained in the Scene_Main.
    /// </summary>
    public static class Bootstrap
    {
        /// <summary>
        /// Creation of the <see cref="GameInstance"/> and start the game.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static async void Initialize()
        {
            var gameInstance = new GameInstance(GameInstanceInitializationConfig.Instance);
            await gameInstance.LoadGame();
        }
    }
}
