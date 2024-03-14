namespace QRCode.Engine.Core.GameState
{
	using QRCode.Engine.Core.GameLevels;
	using QRCode.Engine.Toolbox.Pattern.StateMachine;

	public class GameStateLevelLoader : State
	{
		private GameLevelLoader _gameLevelLoader = null;
		
		public GameStateLevelLoader(int id, string stateName, GameLevelLoader gameLevelLoader) : base(id, stateName)
		{
			_gameLevelLoader = gameLevelLoader;
		}

		public override void OnEnter()
		{
			base.OnEnter();
			ChangeLevelAsync();
		}

		private async void ChangeLevelAsync()
		{
			await _gameLevelLoader.ChangeLevelAsync();
		}
	}
}