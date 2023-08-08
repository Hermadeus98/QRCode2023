namespace QRCode.Engine.Core
{
    using UnityEngine;

    /// <summary>
    /// <see cref="Bootstrap"/> represents the first entry point of the application.
    /// This is the only one admitted entry point of all the application, it must be contained in the Scene_Main.
    /// </summary>
    [RequireComponent(typeof(GameInstanceInitializationDataComponent))]
    public sealed class Bootstrap : MonoBehaviour
    {
        private void Awake()
        {
            var gameInstance = new GameInstance(GetComponent<GameInstanceInitializationDataComponent>());
        }
    }
}
