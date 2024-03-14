namespace QRCode.Engine.Core.GameState
{
    using System.Threading;
    using System.Threading.Tasks;
    using QRCode.Engine.Core.GameLevels;
    using QRCode.Engine.Core.Manager;
    using QRCode.Engine.Toolbox.Pattern.StateMachine;
    using UnityEngine;

    /// <summary>
    /// This class manage all the game states of the application.
    /// </summary>
    public class GameStateManager : GenericManagerBase<GameStateManager>
    {
        #region Fields
        #region Serialized
        [SerializeField] private GameLevelLoader _gameLevelLoader = null;
        #endregion Serialized
        
        #region Internals
        private FiniteStateMachine _gameStateMachine = null;
        #endregion Internals
        #endregion Fields

        #region Methods
        #region Lifecycle
        protected override Task InitAsync(CancellationToken cancellationToken)
        {
            _gameStateMachine = new FiniteStateMachine();
            
            EngineStateInitialization engineStateInitialization = new EngineStateInitialization(EngineStatesConstants.EngineStateInitializationId, EngineStatesConstants.EngineStateInitializationName, _gameStateMachine);
            GameStateLevelLoader gameStateFirstLevel = new GameStateLevelLoader(EngineStatesConstants.GameStateFirstLevelId, EngineStatesConstants.GameStateFirstLevelName, _gameLevelLoader);
            
            _gameStateMachine.AddState(engineStateInitialization);
            _gameStateMachine.AddState(gameStateFirstLevel);
            
            _gameStateMachine.AddLink(engineStateInitialization, gameStateFirstLevel);
            
            _gameStateMachine.StartStateMachine(EngineStatesConstants.EngineStateInitializationId);
            
            return Task.CompletedTask;
        }

        private void Update()
        {
            if (_gameStateMachine != null)
            {
                _gameStateMachine.UpdateStateMachine(Time.deltaTime);
            }
        }

        public override void Delete()
        {
            if (_gameStateMachine != null)
            {
                _gameStateMachine.Delete();
                _gameStateMachine = null;
            }
            
            base.Delete();
        }
        #endregion Lifecycle
        #endregion Methods
    }
}
