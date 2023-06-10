namespace QRCode.Framework
{
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
            public const string SceneManagerSettingsPath = BasePath + "Scene Manager Settings";
            public const string SaveSystemSettingsPath = BasePath + "Save System Settings";
            public const string AudioSettingsPath = BasePath + "Audio Settings";
        }
        
        public static class DebuggingChannels
        {
            public const string Editor = "Editor";
            public const string LifeCycle = "Life Cycle";
            public const string LevelManager = "Level Manager";
            public const string SceneManager = "Scene Manager";
            public const string Database = "Database";
            public const string Misc = "Misc";
            public const string Tests = "Tests";
            public const string Game = "Game";
            public const string Error = "Error";
            public const string Inputs = "Inputs";
            public const string SaveSystem = "Save System";
            public const string Audio = "Audio";
            public const string UserSettings = "User Settings";
            public const string Subtitles = "Subtitles";
            public const string Importer = "Importer";
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

        public static class Catalog
        {
            public const string NamePrefix = "Catalog_";
            public const string BasePath = "QRCode/Catalog/";
            public const string CatalogPath = BasePath + "New Catalog";
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
