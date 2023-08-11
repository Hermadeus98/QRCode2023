namespace QRCode.Engine.Core.GameMode
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Framework.Debugging;

    /// <summary>
    /// This class must contains game rules, player count, how players spawns.
    /// </summary>
    public abstract class GameModeBase : IGameMode
    {
        private List<IGameModeModule> m_gameModeModules;

        public async Task ConstructGameMode(params IGameModeModule[] gameModeModules)
        {
            m_gameModeModules = new List<IGameModeModule>();

            for (int i = 0; i < gameModeModules.Length; i++)
            {
                m_gameModeModules.Add(gameModeModules[i]);
                gameModeModules[i].OnConstruct(this);
            }

            for (int i = 0; i < m_gameModeModules.Count; i++)
            {
                await m_gameModeModules[i].InitAsync(this);
            }
        }

        public T GetGameModeModule<T>() where T : IGameModeModule
        {
            var gameModeModuleTypeToSearch = typeof(T);
            var gameModeModuleCount = m_gameModeModules.Count;
            for (int i = 0; i < gameModeModuleCount; i++)
            {
                if (m_gameModeModules[i].GetType() == gameModeModuleTypeToSearch)
                {
                    return (T)m_gameModeModules[i];
                }
            }
            
            QRDebug.DebugFatal(Constants.EngineConstants.EngineLogChannels.EngineChannel, $"Impossible to find {typeof(T)} in this game mode..");
            return default(T);
        }
    }
}
