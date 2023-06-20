namespace QRCode.Framework
{
    using Game;
    using Sirenix.OdinInspector;

    public abstract class GameplayComponent : SerializedMonoBehaviour, ILoadableObject, ISavableObject
    {
        protected virtual void OnEnable()
        {
            GameInstance.Instance.RegisterGameplayComponent(this);
        }

        protected virtual void OnDisable()
        {
            GameInstance.Instance.UnregisterGameplayComponent(this);
        }

        public virtual void OnLevelLoaded() { }

        public virtual void OnLevelUnloaded() { }

        public virtual void OnGameStart() { }

        public virtual void OnGameUpdate() { }

        public virtual void OnGameEnd() { }
        
        public virtual void OnGameRestart() { }

        public virtual void OnGamePause(PauseInfo pauseInfo) { }
        
        public virtual void LoadGameData(GameData gameData) { }

        public virtual void SaveGameData(ref GameData gameData) { }
    }
}