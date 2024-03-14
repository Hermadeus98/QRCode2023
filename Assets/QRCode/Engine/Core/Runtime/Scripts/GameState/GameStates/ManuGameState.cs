namespace QRCode.Engine.Core.GameState
{
	using QRCode.Engine.Toolbox.Pattern.StateMachine;

	/// <summary>
	/// A state with only UI, no GameMode and no GameLevel are loaded.
	/// </summary>
	public abstract class ManuGameState  : State
	{
		protected ManuGameState(int id, string stateName) : base(id, stateName)
		{
		}
	}
}