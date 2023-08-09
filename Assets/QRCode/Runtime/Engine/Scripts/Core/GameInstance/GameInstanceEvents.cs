namespace QRCode.Engine.Core
{
    using System.Collections.Generic;
    using Framework;
    using Framework.Debugging;
    using K = Framework.K;

    public class GameInstanceEvents
    {
        private List<GameplayComponent> m_gameplayComponents = new List<GameplayComponent>();

        public void RegisterGameplayComponent(GameplayComponent gameplayComponent)
        {
            if (!m_gameplayComponents.Contains(gameplayComponent))
            {
                m_gameplayComponents.Add(gameplayComponent);
            }
        }

        public void UnregisterGameplayComponent(GameplayComponent gameplayComponent)
        {
            if (m_gameplayComponents.Contains(gameplayComponent))
            {
                m_gameplayComponents.Remove(gameplayComponent);
            }
        }
        
        public void OnLevelLoaded()
        {
            for (var i = 0; i < m_gameplayComponents.Count; i++)
            {
                m_gameplayComponents[i].OnLevelLoaded();
            }
            
            QRDebug.DebugTrace(K.DebuggingChannels.Game, $"On Level Loaded.");
        }
        
        public void OnLevelUnloaded()
        {
            for (var i = 0; i < m_gameplayComponents.Count; i++)
            {
                m_gameplayComponents[i].OnLevelUnloaded();
            }
            
            QRDebug.DebugTrace(K.DebuggingChannels.Game, $"On Level Unloaded.");
        }
    }
}