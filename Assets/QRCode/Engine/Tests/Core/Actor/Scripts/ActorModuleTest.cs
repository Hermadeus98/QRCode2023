namespace QRCode.Engine.Tests.Core.Actor.Tests
{
	using System;
	using QRCode.Engine.Core.Actor;
	using Sirenix.OdinInspector;

	[Serializable]
	public class ActorModuleTest : AActorModule
	{
		[ShowInInspector] private int _valueTest = 454;
		
		public ActorModuleTest() : base()
		{
		}
	}
}