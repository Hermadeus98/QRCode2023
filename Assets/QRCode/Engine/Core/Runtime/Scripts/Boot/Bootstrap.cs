namespace QRCode.Engine.Core.Boot
{
    using GameInstance;
    using UnityEngine;
    
    /// <summary>
    /// <see cref="Bootstrap"/> represents the first entry point of the application.
    /// This is the only one admitted entry point of all the application, it must be contained in the Scene_Main.
    /// </summary>
    public class Bootstrap : MonoBehaviour
    {
        private void Awake()
        {
            Initialize();
        }

        /// <summary>
        /// Creation of the <see cref="GameInstance"/> and start the game.
        /// </summary>
        private async void Initialize()
        {
            var gameInstance = new GameInstance(GameInstanceInitializationConfig.Instance);

            Boot boot = new Boot();
            await boot.Execute();
            boot.Delete();
         
            await gameInstance.LoadGame();
        }
    }
}
