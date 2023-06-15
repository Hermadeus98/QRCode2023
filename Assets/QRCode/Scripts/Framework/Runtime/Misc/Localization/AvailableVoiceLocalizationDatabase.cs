namespace QRCode.Framework
{
    using UnityEngine;
    using UnityEngine.Localization;

    [CreateAssetMenu(menuName = K.DatabasePath.BasePath + "Available Voice Localization Database", fileName = "DB_AvailableVoiceLocales")]
    public class AvailableVoiceLocalizationDatabase : ScriptableObjectDatabase<LocaleIdentifier>
    {
        
    }
}