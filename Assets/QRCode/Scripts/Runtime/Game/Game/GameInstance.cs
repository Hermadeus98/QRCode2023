namespace QRCode.Framework.Game
{
    using System;
    using System.Collections.Generic;
    using Framework;
    using Framework.Debugging;
    using K = Framework.K;

    public class GameInstance
    {
        public static GameInstance Instance = null;
        
        private PauseInfo m_pauseInfo = new PauseInfo();
        private GameInfo m_gameInfo = new GameInfo();
        private StateMachine m_gameStateMachine;

        private Action<PauseInfo> m_onGamePaused;

        private List<GameplayComponent> m_gameplayComponents = new List<GameplayComponent>();

        public GameInstance()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                QRDebug.DebugFatal(K.DebuggingChannels.Game, $"There is already an instance of Game.");
            }
        }

        public static void Create()
        {
            if (GameInstance.Instance == null)
            {
                var game = new GameInstance();
            }
        }

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

        public void PreInitialize()
        {
            var gameStates = new IState[]
            {
                new GameState_Game(),
                new GameState_Pause(),
                new GameState_SplashScreen(),
                new GameState_MainMenu(),
            };
            
            m_gameStateMachine = new StateMachine(K.Game.GameState_SplashScreen, UpdateModeEnum.Update, gameStates);
        }
        
        public void SetGamePause(bool value)
        {
            QRDebug.DebugTrace(K.DebuggingChannels.Game, $"Game Pause : {value}.");
            
            m_pauseInfo.Pause = value;
            m_onGamePaused?.Invoke(m_pauseInfo);
            
            for (var i = 0; i < m_gameplayComponents.Count; i++)
            {
                m_gameplayComponents[i].OnGamePause(m_pauseInfo);
            }
        }

        public void OnLevelLoaded()
        {
            QRDebug.DebugTrace(K.DebuggingChannels.Game, $"Level Loaded.");
            
            for (var i = 0; i < m_gameplayComponents.Count; i++)
            {
                m_gameplayComponents[i].OnLevelLoaded();
            }
        }
        
        public void OnLevelUnloaded()
        {
            QRDebug.DebugTrace(K.DebuggingChannels.Game, $"Level Unloaded.");
            
            for (var i = 0; i < m_gameplayComponents.Count; i++)
            {
                m_gameplayComponents[i].OnLevelUnloaded();
            }
        }

        public bool IsInPause() => m_pauseInfo.Pause;
    }

    public struct GameInfo
    {
        public bool GameIsStarted { get; set; }
    }
    
    public struct PauseInfo
    {
        public bool Pause { get; set; }
    }
}
