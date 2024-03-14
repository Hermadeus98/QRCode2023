namespace QRCode.Engine.Core.Actor
{
	using UnityEngine;
	using UnityEngine.AddressableAssets;

	/// <summary>
	/// This struct is a container of parameter to create a new actor from <see cref="ActorFactory"/>.
	/// </summary>
	public struct ActorCreationParameters
	{
		public AActorModule[] ActorModules { get; }
		public AssetReference ActorGameObjectPrefab { get; }
		public Vector3 SpawnPosition { get; }
		public Quaternion SpawnRotation { get; }
		public Transform ActorParent { get; }

		/// <param name="actorModules">all the modules that define the actor.</param>
		/// <param name="actorGameObjectPrefab">the prefab reference.</param>
		/// <param name="spawnPosition">the world spawn position.</param>
		/// <param name="spawnRotation">the initial actor rotation.</param>
		/// <param name="actorParent">the parent transform in the hierarchy.</param>
		public ActorCreationParameters(
			AActorModule[] actorModules,
			AssetReference actorGameObjectPrefab,
			Vector3 spawnPosition,
			Quaternion spawnRotation,
			Transform actorParent)
		{
			ActorModules = actorModules;
			ActorGameObjectPrefab = actorGameObjectPrefab;
			SpawnPosition = spawnPosition;
			SpawnRotation = spawnRotation;
			ActorParent = actorParent;
		}
	}
}