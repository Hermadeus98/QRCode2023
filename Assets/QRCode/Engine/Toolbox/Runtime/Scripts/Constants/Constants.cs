namespace QRCode.Engine.Toolbox
{
    public static partial class Constants
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
            public const string Infos = "Infos";
        }
        
        public static class SmartStringPath
        {
            private const string CreateAssetMenuBasePath = "QRCode/Smart Strings Reference/";
            public const string SmartStringReference = CreateAssetMenuBasePath + "Smart String Reference";
            public const string SmartInputIconReference = CreateAssetMenuBasePath + "Smart Input Icon Reference";
        }
        
        public static class SettingsPath
        {
            private const string BasePath = "QRCode/Settings/";

            public const string ServiceSettingsPath = BasePath + "Service Settings";
            public const string InputSettingsPath = BasePath + "Input Settings";
            public const string GameLevelManagerSettingsPath = BasePath + "Game Level Manager Settings";
            public const string SaveSystemSettingsPath = BasePath + "Save System Settings";
            public const string AudioSettingsPath = BasePath + "Audio Settings";
            public const string LocalizationPath = BasePath + "Localization Settings";
            public const string InterfaceSettings = BasePath + "Interface Settings";
            public const string RemoteConfigSettings = BasePath + "Remote Config Settings";
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
            public const string Inputs = BasePath + "/Inputs/";
        }
        
        public static class CreateMenuItemPath
        {
            private const string BasePath = "GameObject/QRCode/";

            public const string UI = BasePath + "UI/";
            public const string UIUtilities = UI + "/Utilities/";
        }
        
        public static class UI
        {
            private const string BasePath = "QRCode/UI/";

            public const string UIRuleSets = BasePath + "UI Rule Sets/";
        }
        
        public static class MenuNameTestPath
        {
            private const string BasePath = "QRCode/";
            public const string Test = BasePath + "Tests/";
        }

        public static class GameConfigs
        {
            public const string NamePrefix = "GC_";
            public const string BasePath = "QRCode/Game Configs/";
            public const string GameConfigsPath = BasePath + "New Game Congig";
        }
        
        public static class Subtitles
        {
            private const string BasePath = "QRCode/Subtitles/";

            public const string SubtitleDataPath = BasePath + "Subtitle Data";

            public const string SubtitleTextPlaceHolder = "This is a place holder text !!!";
            public const string SubtitleSpeakerNamePlaceHolder = "SpeakerName";
        }
    }
}
