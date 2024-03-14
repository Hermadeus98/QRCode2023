namespace QRCode.Engine.Core.GameState
{
	using QRCode.Engine.Core.GameInstance;
	using QRCode.Engine.Toolbox.Pattern.StateMachine;

	public class EngineStateInitialization : State
	{
		private FiniteStateMachine _gameStateMachine = null;

		public EngineStateInitialization(int id, string stateName, FiniteStateMachine gameStateMachine) : base(id, stateName)
		{
			_gameStateMachine = gameStateMachine;
		}

		public override void OnUpdate(float deltaTime)
		{
			if (GameInstance.Instance.IsReady)
			{
				_gameStateMachine.TryChangeState(EngineStatesConstants.GameStateFirstLevelId);
			}
			
			base.OnUpdate(deltaTime);
		}
	}
}