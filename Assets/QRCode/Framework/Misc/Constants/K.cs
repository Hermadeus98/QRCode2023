namespace QRCode.Framework
{
    using Game;

    public static partial class K
    {
        public static class InspectorGroups
        {
            public const string SingletonSettingsGroup = "Singleton Settings";

            public const string References = "References";
            public const string Balancing = "Balancing";
            public const string Settings = "Settings";
            public const string Cosmetics = "Cosmetics";
            public const string GameEvents = "GameEvents";
            public const string Debugging = "Debugging";
        }
        
        public static class DebuggingPath
        {
            public const string CreateAssetMenuBasePath = "QRCode/Debugging/";
            public const string ChannelDebugPath = CreateAssetMenuBasePath + "Channels Settings";
            public const string ScriptableDebugPath = CreateAssetMenuBasePath + "Scriptable Object Debugger";
        }
        
        public static class DebuggingChannels
        {
            public const string Editor = "Editor";
            public const string LifeCycle = "Life Cycle";
            public const string SceneManager = "Scene Manager";
            public const string Database = "Database";
            public const string Misc = "Misc";
            public const string Tests = "Tests";
            public const string Game = "Game";
            public const string Error = "Error";
        } 
        
        public static class Game
        {
            public const string GameState_Game = "GameState_Game";
            public const string GameState_Pause = "GameState_Pause";
            public const string GameState_MainMenu = "GameState_MainMenu";
            public const string GameState_SplashScreen = "GameState_SplashScreen";
        }
        
        public static class DatabasePath
        {
            public const string BasePath = "QRCode/Databases/";
        }
    }
}
