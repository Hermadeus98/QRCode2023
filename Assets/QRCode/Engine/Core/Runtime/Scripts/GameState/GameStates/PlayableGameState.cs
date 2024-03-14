namespace QRCode.Engine.Core.GameState
{
	using QRCode.Engine.Toolbox.Pattern.StateMachine;

	/// <summary>
	/// Game State that where the current GameMode and the current GameLevel is not in pause.
	/// </summary>
	public class PlayableGameState : State
	{
		public PlayableGameState(int id, string stateName) : base(id, stateName)
		{
		}
	}
}