namespace QRCode.Engine.Core.GameState
{
	using QRCode.Engine.Toolbox.Pattern.StateMachine;

	/// <summary>
	/// A state where the current GameLevel and the current GameMode is paused.
	/// </summary>
	public class PausableGameState : State
	{
		public PausableGameState(int id, string stateName) : base(id, stateName)
		{
		}
	}
}