namespace QRCode.Engine.Core.RemoteConfig
{
    using Unity.Services.Core;
    using Unity.Services.Authentication;
    using Unity.Services.RemoteConfig;

    using System;
    using System.Threading;
    using System.Threading.Tasks;
    
    using Sirenix.OdinInspector;

    using Debugging;
    using Managers;
    using Toolbox;
    using Toolbox.Pattern.Singleton;

    public class RemoteConfigManager : MonoBehaviourSingleton<RemoteConfigManager>, IManager
    {
        private struct UserAttributes
        {
            
        }

        private struct AppAttributes
        {
            
        }

        private RemoteConfigSettings m_remoteConfigSettings;
        private RemoteConfigEvents m_remoteConfigEvents;
        
        private UserAttributes m_userAttributes;
        private AppAttributes m_appAttributes;

        private bool m_hasBeenFetch = false;

        public async Task InitAsync(CancellationToken cancellationToken)
        {
            m_userAttributes = new UserAttributes();
            m_appAttributes = new AppAttributes();

            m_remoteConfigSettings = RemoteConfigSettings.Instance;
            
            CreateRemoteConfigEvents();

            do
            {
                try
                {
                    await InitializeRemoteConfigServiceAsync();
                    await FetchRemoteConfig();
                    await Task.Yield();
                }
                catch (Exception e)
                {
                    if (e is RequestFailedException requestFailedException)
                    {
                        HandleRequestFailedException();
                    }
                    else
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                }
            } 
            while (m_hasBeenFetch == false && cancellationToken.IsCancellationRequested == false);
        }

        [Button]
        public async void FetchRemoteConfigAsync()
        {
            await FetchRemoteConfig();
        } 
        
        private async Task InitializeRemoteConfigServiceAsync()
        {
            await UnityServices.InitializeAsync();

            if (AuthenticationService.Instance.IsSignedIn == false)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }
        }

        private async Task FetchRemoteConfig()
        {
            await RemoteConfigService.Instance.FetchConfigsAsync(m_userAttributes, m_appAttributes);
            RemoteConfigService.Instance.SetEnvironmentID(m_remoteConfigSettings.EnvironmentId);
            RemoteConfigService.Instance.FetchCompleted += OnFetchCompleted;
        }

        private void HandleRequestFailedException()
        {
            FetchAllRemoteConfigValues();
        }

        private void OnFetchCompleted(ConfigResponse configResponse)
        {
            switch (configResponse.requestOrigin)
            {
                case ConfigOrigin.Default:
                    QRDebug.DebugInfo(Constants.DebuggingChannels.RemoteConfig, $"No settings loaded this session, using default values.");
                    break;
                case ConfigOrigin.Cached:
                    QRDebug.DebugInfo(Constants.DebuggingChannels.RemoteConfig, $"No settings loaded this session, using cached values from a previous version.");
                    break;
                case ConfigOrigin.Remote:
                    QRDebug.DebugInfo(Constants.DebuggingChannels.RemoteConfig, $"New settings loaded this session, update values accordingly.");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            FetchAllRemoteConfigValues();
            QRDebug.DebugInfo(Constants.DebuggingChannels.RemoteConfig, $"Fetch completed : AppVersion = {m_remoteConfigSettings.AppVersion}.");
        }

        private void CreateRemoteConfigEvents()
        {
            m_remoteConfigEvents = new RemoteConfigEvents(m_remoteConfigSettings.AllRemoteConfigValues);
        }
        
        private void FetchAllRemoteConfigValues()
        {
            var allRemoteConfigValues = m_remoteConfigSettings.AllRemoteConfigValues;
            var remoteConfigValuesCount = allRemoteConfigValues.Length;
            var runtimeConfig = RemoteConfigService.Instance.appConfig;
            for (int i = 0; i < remoteConfigValuesCount; i++)
            {
                allRemoteConfigValues[i].FetchValue(runtimeConfig);
                RaiseOnValueChangeEvent(allRemoteConfigValues[i]);
            }
            
            m_hasBeenFetch = true;
        }

        public void RegisterOnValueFetched(RemoteConfigValueBase remoteConfigValueBase, RemoteConfigEvents.OnValueUpdated action)
        {
            m_remoteConfigEvents.RegisterDelegate(remoteConfigValueBase, action);
        }
        
        public void UnregisterOnValueFetched(RemoteConfigValueBase remoteConfigValueBase, RemoteConfigEvents.OnValueUpdated action)
        {
            m_remoteConfigEvents.UnregisterDelegate(remoteConfigValueBase, action);
        }
        
        private void RaiseOnValueChangeEvent(RemoteConfigValueBase remoteConfigValue)
        {
            m_remoteConfigEvents.RaiseOnValueChangedEvent(remoteConfigValue);
        }
    }
}
