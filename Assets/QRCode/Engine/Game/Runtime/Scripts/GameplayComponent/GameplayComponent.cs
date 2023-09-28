namespace QRCode.Engine.Core
{
    using GameInstance;
    using SaveSystem;
    using Sirenix.OdinInspector;
    using Toolbox.Optimization;

    public abstract class GameplayComponent : SerializedMonoBehaviour, IGameplayComponent , ILoadableObject, ISavableObject, IDeletable
    {
        protected virtual void OnEnable()
        {
            GameInstance.GameInstance.Instance.GameInstanceEvents.RegisterGameplayComponent(this);
        }

        protected virtual void OnDisable()
        {
            GameInstance.GameInstance.Instance?.GameInstanceEvents?.UnregisterGameplayComponent(this);
        }

        public virtual void OnGameInstanceIsReady()
        {
            
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

        protected virtual void OnDestroy()
        {
            Delete();
        }

        public void Delete()
        {
            
        }
    }
}
