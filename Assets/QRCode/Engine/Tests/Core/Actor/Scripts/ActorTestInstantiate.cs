namespace QRCode.Engine.Tests.Core.Actor.Tests
{
	using QRCode.Engine.Core.Actor;
	using Sirenix.OdinInspector;
	using Unity.Mathematics;
	using UnityEngine;
	using UnityEngine.AddressableAssets;
	using Random = UnityEngine.Random;

	public class ActorTestInstantiate : MonoBehaviour
	{
		[SerializeField] private AssetReference actorAssetReference = null;

		[Button]
		private async void Instantiate()
		{
			Vector3 position = Random.insideUnitSphere * 10.0f;
			Quaternion rotation = quaternion.identity;

			AActorModule[] actorModuleTests = {
				new ActorModuleTest(),
				new ActorModuleTest(),
				new ActorModuleTest(),
			};
			
			await ActorFactory.Instance.TryCreateActor<ActorTest>(actorModuleTests, actorAssetReference, position, rotation);
		}
	}
}