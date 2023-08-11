namespace QRCode.Engine.Core.Player
{
    using Framework;

    public class PlayerProfile
    {
        public UserSettingsData UserSettingsData => UserSettingsManager.Instance.GetUserSettingsData();
    }
}