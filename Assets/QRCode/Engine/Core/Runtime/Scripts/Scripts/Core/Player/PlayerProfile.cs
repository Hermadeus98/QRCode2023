namespace QRCode.Engine.Core.Player
{
    using UserSettings;

    public class PlayerProfile
    {
        public UserSettingsData UserSettingsData => UserSettingsManager.Instance.GetUserSettingsData();
    }
}