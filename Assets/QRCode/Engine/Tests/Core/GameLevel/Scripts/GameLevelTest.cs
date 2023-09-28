namespace QRCode.Engine.Core.GameLevel.Tests
{
    using GameLevels;

    /// <summary>
    /// A <see cref="GameLevel"/> example.
    /// </summary>
    public class GameLevelTest : AGameLevel
    {
        protected override void BuildGameLevelModules()
        {
            if (aGameLevelData.TryGetGameLevelModuleOfType<GameLevelModuleLoadableTestData>(out var gameLevelModuleLoadableTestData))
            {
                AddGameLevelModule(new GameLevelModuleLoadableTest(gameLevelModuleLoadableTestData));
            }
        }
    }
}
