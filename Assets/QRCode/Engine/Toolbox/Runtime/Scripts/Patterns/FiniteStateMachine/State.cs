namespace QRCode.Engine.Toolbox.Pattern.StateMachine
{
	using QRCode.Engine.Debugging;
	using QRCode.Engine.Toolbox.Optimization;
	using QRCode.Engine.Toolbox.Tags;

	/// <summary>
	/// A state that can be used by a State Machine.
	/// </summary>
	public abstract class State : IDeletable
	{
		#region Fields
		protected int _stateId = 0;
		protected string _stateName = string.Empty;
		#endregion Fields

		#region Properties
		public int Id { get { return _stateId; } }
		public string Name { get { return _stateName; } }
		#endregion Properties

		#region Constructor
		public State(int id, string stateName)
		{
			_stateId = id;
			_stateName = stateName;
		}
		#endregion Constructor

		#region Methods
		#region LifeCycle
		public virtual void Delete()
		{
			
		}
		#endregion LifeCycle

		#region Public Methods
		/// <summary>
		/// Executed code when the state machine enter in this state.
		/// </summary>
		public virtual void OnEnter()
		{
			QRLogger.Debug<ToolboxTags.Patterns.FiniteStateMachine>($"Entering in state {Name}.");
		}

		/// <summary>
		/// Executed code when the current state of the state machine is this state.
		/// </summary>
		public virtual void OnUpdate(float deltaTime)
		{
			
		}

		/// <summary>
		/// Executed code when the state machine exit of this state.
		/// </summary>
		public virtual void OnExit()
		{
			QRLogger.Debug<ToolboxTags.Patterns.FiniteStateMachine>($"Exiting in state {Name}.");
		}
		#endregion Public Methods
		#endregion Methods
	}
}