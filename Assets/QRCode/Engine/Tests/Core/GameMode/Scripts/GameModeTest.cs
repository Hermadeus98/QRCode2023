namespace QRCode.Engine.Core.GameMode.Tests
{
	using System.Collections.Generic;

	public class GameModeTest : AGameMode
	{
		protected override List<AGameModeModule> CreateGameModeModules()
		{
			List<AGameModeModule> modules = new List<AGameModeModule>();
			
			GameModeModuleTest gameModeModuleTest = new GameModeModuleTest();
			modules.Add(gameModeModuleTest);

			return modules;
		}
	}
}