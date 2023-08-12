namespace QRCode.Engine.Core.Localization
{
    using Toolbox;
    using Toolbox.Database;
    using UnityEngine;
    using UnityEngine.Localization;

    [CreateAssetMenu(menuName = Constants.DatabasePath.BasePath + "Available Voice Localization Database", fileName = "DB_AvailableVoiceLocales")]
    public class AvailableVoiceLocalizationDatabase : ScriptableObjectDatabase<LocaleIdentifier>
    {
        protected override string m_databaseInformation { get => "All voices locales available in the game."; }
    }
}