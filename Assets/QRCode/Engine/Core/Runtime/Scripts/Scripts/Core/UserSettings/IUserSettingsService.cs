namespace QRCode.Engine.Core.UserSettings
{
    using System.Threading.Tasks;
    using Toolbox.Pattern.Service;

    public interface IUserSettingsService : IService
    {
        public Task InitAsync();
        public UserSettingsData GetUserSettingsData();
        public Task CreateUserSettingsData();
        public Task LoadUserSettingsData();
        public Task SaveUserSettingsData();
        public void ApplyChange(UserSettingsData newUserSettingsData = null);
    }
}
