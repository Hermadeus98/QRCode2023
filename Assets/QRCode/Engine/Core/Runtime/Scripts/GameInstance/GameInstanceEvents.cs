namespace QRCode.Engine.Core.GameInstance
{
    using System.Collections.Generic;
    using Toolbox;
    using Debugging;
    using QRCode.Engine.Core.Tags;
    using Constants = Toolbox.Constants;

    public class GameInstanceEvents
    {
        private List<IGameplayComponent> m_gameplayComponents = new List<IGameplayComponent>();

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

        public void DeleteAll()
        {
            for (int i = 0; i < m_gameplayComponents.Count; i++)
            {
                m_gameplayComponents[i].Delete();
            }
            
            QRLogger.DebugTrace<CoreTags.GameInstance>($"On Level Unloaded.");
        }
    }

    public struct PauseInfo
    {
    }
}