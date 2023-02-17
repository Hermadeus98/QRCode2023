namespace QRCode.Framework.Game
{
    using System;
    using Framework;
    using Framework.Debugging;

    public static class Game
    {
        private static PauseInfo m_pauseInfo = new PauseInfo();
        private static GameInfo m_gameInfo = new GameInfo();

        private static Action m_onInitialize;
        private static Action m_onGameStart;
        private static Action<PauseInfo> m_onGamePaused;

        public static event Action OnInitialize
        {
            add
            {
                m_onInitialize -= value;
                m_onInitialize += value;
            }
            remove
            {
                m_onInitialize -= value;
            }
        }
        
        public static event Action OnGameStart
        {
            add
            {
                m_onGameStart -= value;
                m_onGameStart += value;
            }
            remove
            {
                m_onGameStart -= value;
            }
        }
        
        public static event Action<PauseInfo> OnGamePaused
        {
            add
            {
                m_onGamePaused -= value;
                m_onGamePaused += value;
            }
            remove
            {
                m_onGamePaused -= value;
            }
        }

        private static StateMachine m_gameStateMachine;

        public static void PreInitialize()
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
        
        public static void Initialize()
        {
            QRDebug.DebugTrace(K.DebuggingChannels.Game, $"Game Pre-initialize.");
            
            m_onInitialize?.Invoke();
        }
        
        public static void StartGame()
        {
            QRDebug.DebugTrace(K.DebuggingChannels.Game, $"Game Start.");
            
            m_gameInfo.GameIsStarted = true;
            m_onInitialize?.Invoke();
            m_onGameStart?.Invoke();
        }
        
        public static void SetGamePause(bool value)
        {
            QRDebug.DebugTrace(K.DebuggingChannels.Game, $"Game Pause : {value}.");
            
            m_pauseInfo.Pause = value;
            m_onGamePaused?.Invoke(m_pauseInfo);
        }

        public static bool IsInPause() => m_pauseInfo.Pause;
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
