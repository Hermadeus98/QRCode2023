namespace QRCode.Engine.Core.GameInstance
{
    using System.Collections.Generic;
    using Toolbox;
    using Debugging;

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
        
        public void OnLevelLoaded()
        {
            for (var i = 0; i < m_gameplayComponents.Count; i++)
            {
                m_gameplayComponents[i].OnLevelLoaded();
            }
            
            QRDebug.DebugTrace(Constants.DebuggingChannels.Game, $"On Level Loaded.");
        }
        
        public void OnLevelUnloaded()
        {
            for (var i = 0; i < m_gameplayComponents.Count; i++)
            {
                m_gameplayComponents[i].OnLevelUnloaded();
            }
            
            QRDebug.DebugTrace(Constants.DebuggingChannels.Game, $"On Level Unloaded.");
        }
    }

    public struct PauseInfo
    {
    }
}