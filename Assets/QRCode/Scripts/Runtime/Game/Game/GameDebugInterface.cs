namespace QRCode.Framework.Game
{
    using Framework.Singleton;
    using Sirenix.OdinInspector;
    
    public class GameDebugInterface : MonoBehaviourSingleton<GameDebugInterface>
    {
        [ButtonGroup("Debugging")]
        private void SetGamePause(bool value) => GameInstance.Instance.SetGamePause(value);
    }
}
