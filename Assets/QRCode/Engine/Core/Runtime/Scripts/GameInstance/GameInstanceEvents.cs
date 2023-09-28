namespace QRCode.Engine.Core.GameInstance
{
    using System.Collections.Generic;
    using Debugging;
    using QRCode.Engine.Core.Tags;
    using QRCode.Engine.Toolbox.Optimization;

    public class GameInstanceEvents : IDeletable
    {
        private List<IGameplayComponent> m_gameplayComponents = null;

        public GameInstanceEvents()
        {
            m_gameplayComponents = new List<IGameplayComponent>();
        }
        
        public void RegisterGameplayComponent(IGameplayComponent gameplayComponent)
        {
            if (!m_gameplayComponents.Contains(gameplayComponent))
            {
                m_gameplayComponents.Add(gameplayComponent);
            }
        }

        public void UnregisterGameplayComponent(IGameplayComponent gameplayComponent)
        {
            if (m_gameplayComponents.Contains(gameplayComponent))
            {
                m_gameplayComponents.Remove(gameplayComponent);
            }
        }

        public void OnGameInstanceIsReady()
        {
            for (int i = 0; i < m_gameplayComponents.Count; i++)
            {
                m_gameplayComponents[i].OnGameInstanceIsReady(); 
            }
            
            QRLogger.DebugTrace<CoreTags.GameInstance>($"On Game Instance is ready.");
        }
        
        public void OnLevelLoaded()
        {
            for (var i = 0; i < m_gameplayComponents.Count; i++)
            {
                m_gameplayComponents[i].OnLevelLoaded();
            }
            
            QRLogger.DebugTrace<CoreTags.GameInstance>($"On Level Loaded.");
        }
        
        public void OnLevelUnloaded()
        {
            for (var i = 0; i < m_gameplayComponents.Count; i++)
            {
                m_gameplayComponents[i].OnLevelUnloaded();
            }
            
            QRLogger.DebugTrace<CoreTags.GameInstance>( $"On Level Unloaded.");
        }

        public void Delete()
        {
            if (m_gameplayComponents != null)
            {
                for (int i = 0; i < m_gameplayComponents.Count; i++)
                {
                    m_gameplayComponents[i].Delete();
                }
            }
        }
    }

    public struct PauseInfo
    {
    }
}