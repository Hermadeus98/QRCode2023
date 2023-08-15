namespace QRCode.Engine.Core.GameLevel.Tests
{
    using GameLevels;

    public class GameLevelTest : GameLevel
    {
        public override void BuildGameLevelModules()
        {
            if (m_gameLevelData.TryGetGameLevelModuleOfType<GameLevelModuleLoadableTestData>(
                    out var gameLevelModuleLoadableTestData))
            {
                AddGameLevelModule<GameLevelModuleLoadableTestData>(new GameLevelModuleLoadableTest(gameLevelModuleLoadableTestData));
            }
        }
    }
}
