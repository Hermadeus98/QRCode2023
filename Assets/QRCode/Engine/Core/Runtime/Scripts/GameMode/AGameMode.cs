namespace QRCode.Engine.Core.GameMode
{
    using System;
    using System.Collections.Generic;
    using Debugging;
    using QRCode.Engine.Core.Tags;
    using QRCode.Engine.Toolbox.Optimization;

    /// <summary>
    /// This class must contains game rules, player count, how players spawns.
    /// </summary>
    public abstract class AGameMode : IDeletable
    {
        private List<AGameModeModule> m_gameModeModules = null;

        protected abstract List<AGameModeModule> CreateGameModeModules();
        
        public void ConstructGameMode()
        {
            m_gameModeModules = new List<AGameModeModule>(CreateGameModeModules());
            
            for (int i = 0; i < m_gameModeModules.Count; i++)
            {
                m_gameModeModules[i].OnConstruct(this);
            }

            for (int i = 0; i < m_gameModeModules.Count; i++)
            {
                m_gameModeModules[i].Initialize(this);
            }
        }
        
        public virtual void Delete()
        {
            if (m_gameModeModules != null)
            {
                for (int i = 0; i < m_gameModeModules.Count; i++)
                {
                    m_gameModeModules[i].Delete();
                }
                
                m_gameModeModules.Clear();
                m_gameModeModules = null;
            }
        }

        public bool TryGetModuleOfType<T>(out T module) where T : AGameModeModule
        {
            Type gameModeModuleTypeToSearch = typeof(T);
            int gameModeModuleCount = m_gameModeModules.Count;
            for (int i = 0; i < gameModeModuleCount; i++)
            {
                if (m_gameModeModules[i].GetType() == gameModeModuleTypeToSearch)
                {
                    module = (T)m_gameModeModules[i];
                    return true;
                }
            }
            
            QRLogger.DebugFatal<CoreTags.GameModes>($"Impossible to find {typeof(T)} in this game mode..");
            module = null;
            return false;
        }
    }
}
