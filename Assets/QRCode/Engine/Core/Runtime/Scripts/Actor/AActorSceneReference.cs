namespace QRCode.Engine.Core.Actor
{
	using System.Collections.Generic;
	using QRCode.Engine.Core.Tags;
	using QRCode.Engine.Debugging;
	using Sirenix.OdinInspector;

	/// <summary>
	/// The reference of the actor in the scene, useful to give some references to the actor.
	/// </summary>
	public abstract class AActorSceneReference : SerializedMonoBehaviour
	{
		#region Fields
		private AActor _owner = null;
		#endregion Fields

		#region Properties
		/// <summary>
		/// The <see cref="AActor"/> that owns this <see cref="AActorSceneReference"/>.
		/// </summary>
		protected AActor Owner { get { return _owner; } }
		#endregion Properties

		#region Constructor
		public virtual void Initialize(AActor owner)
		{
			_owner = owner;
		}
		#endregion Constructor

		#region Methods
		#region Editor Methods
#if UNITY_EDITOR
		[ShowInInspector][ReadOnly] private List<AActorModule> _allActorModulesToDebug = new List<AActorModule>();

		[Button("Refresh Modules")]
		private void EditorRefreshModules()
		{
			if (_owner == null)
			{
				QRLogger.DebugError<CoreTags.Actor>("Actor is null.");
				return;
			}
			_allActorModulesToDebug = new List<AActorModule>(_owner.AllActorModules);
		}
#endif
		#endregion Editor Methods
		#endregion Methods

	}
}