namespace QRCode.Engine.Core.Actor
{
	using System;
	using QRCode.Engine.Toolbox.Optimization;
	using Sirenix.OdinInspector;

	/// <summary>
	/// An <see cref="AActor"/> is composed of <see cref="AActorModule"/>.
	/// </summary>
	[Serializable]
	public class AActorModule : IDeletable
	{
		#region Fields
		#region Editor Fields
#if UNITY_EDITOR
		/// <summary>
		/// The module's name used to display debug interface into inspector.
		/// </summary>
		[ShowInInspector] private string _moduleName = string.Empty;
#endif
		#endregion Editor Fields

		#region Internals
		private AActor _owner = null;
		#endregion Internals
		#endregion Fields

		#region Properties
		/// <summary>
		/// The <see cref="AActor"/> that's own this module.
		/// </summary>
		public AActor Owner { get { return _owner; } }
		#endregion Properties

		#region Constructor
		public AActorModule()
		{
#if UNITY_EDITOR
			_moduleName = GetType().Name;
#endif
		}
		#endregion Constructor

		#region Methods
		#region LifeCycle
		public virtual void Delete()
		{
			_owner = null;
		}
		#endregion LifeCycle

		#region Public Methods
		/// <summary>
		/// This function is used to init the Actor.
		/// </summary>
		public void InitActorModuleAsync(AActor owner)
		{
			_owner = owner;
			Initialize();
		}
		#endregion Public Methods

		#region Protected Methods
		/// <summary>
		/// This function must be override to initialize this actor.
		/// </summary>
		protected virtual void Initialize()
		{
			
		}
		#endregion Protected Methods
		#endregion Methods
	}
}