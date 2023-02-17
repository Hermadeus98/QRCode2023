namespace QRCode.Framework.Game
{
    using Framework.Singleton;
    using Sirenix.OdinInspector;
    using UnityEngine;
    
    public class GameDebugInterface : MonoBehaviourSingleton<GameDebugInterface>
    {
        [TitleGroup("Game Settings")]
        [SerializeField] private bool m_startGameOnStart = true;

        private void Start()
        {
            if (m_startGameOnStart)
            {
                StartGame();
            }
        }

        [ButtonGroup("Debugging")]
        private void StartGame() => Game.StartGame();

        [ButtonGroup("Debugging")]
        private void SetGamePause(bool value) => Game.SetGamePause(value);
    }
}
