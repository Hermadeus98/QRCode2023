namespace QRCode.Framework.Game
{
    using Sirenix.OdinInspector;

    public class GameplayMonoBehaviour : SerializedMonoBehaviour
    {
        protected virtual void OnEnable()
        {
            Game.OnInitialize += OnInitialize;
            Game.OnGameStart += OnGameStart;
            Game.OnGamePaused += OnGamePause;
        }

        protected void OnDisable()
        {
            Game.OnInitialize -= OnInitialize;
            Game.OnGameStart -= OnGameStart;
            Game.OnGamePaused -= OnGamePause;
        }

        /// <summary>
        /// Initialize Gameplay Object, it's always call before OnGameStart.
        /// </summary>
        protected virtual void OnInitialize()
        {
            
        }

        /// <summary>
        /// Call back when game is started.
        /// </summary>
        protected virtual void OnGameStart()
        {
            
        }

        /// <summary>
        /// Callback when game is paused.
        /// </summary>
        /// <param name="pauseInfo"></param>
        protected virtual void OnGamePause(PauseInfo pauseInfo)
        {
            
        }
    }
}
