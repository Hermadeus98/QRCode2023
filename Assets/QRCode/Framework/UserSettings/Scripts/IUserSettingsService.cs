namespace QRCode.Framework
{
    using System.Threading.Tasks;

    public interface IUserSettingsService : IService
    {
        public UserSettingsEvents UserSettingsEvents { get; }
        public Task Initialize();
        public UserSettingsData GetUserSettingsData();
        public Task CreateUserSettingsData();
        public Task LoadUserSettingsData();
        public Task SaveUserSettingsData();
        public void ApplyChange();
    }
}
