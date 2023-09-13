namespace QRCode.Engine.Core.RemoteConfig
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using QRCode.Engine.Core.Manager;
    using QRCode.Engine.Core.Tags;
    using QRCode.Engine.Debugging;
    using QRCode.Engine.Toolbox.Optimization;
    using Sirenix.OdinInspector;
    using Unity.Services.Authentication;
    using Unity.Services.Core;
    using Unity.Services.RemoteConfig;
    using UnityEngine;
    using Constants = Toolbox.Constants;

    /// <summary>
    /// This class manage all the remote config values of the game.
    /// </summary>
    public class RemoteConfigManager : GenericManagerBase<RemoteConfigManager>, IDeletable
    {
        #region Internals
        private struct UserAttributes
        {
            public static readonly UserAttributes Default = new UserAttributes();
        }

        private struct AppAttributes
        {
            public static readonly AppAttributes Default = new AppAttributes();
        }
        #endregion Internals

        #region Fields
        private RemoteConfigSettings m_remoteConfigSettings = null;
        private RemoteConfigEvents m_remoteConfigEvents = null;
        
        private UserAttributes m_userAttributes = UserAttributes.Default;
        private AppAttributes m_appAttributes = AppAttributes.Default;

        private bool m_hasBeenFetch = false;
        #endregion Fields

        #region Methods
        #region LifeCycle
        protected override async Task InitAsync(CancellationToken cancellationToken)
        {
            m_userAttributes = new UserAttributes();
            m_appAttributes = new AppAttributes();

            m_remoteConfigSettings = RemoteConfigSettings.Instance;
            m_remoteConfigEvents = new RemoteConfigEvents(m_remoteConfigSettings.AllRemoteConfigValues);
            
            while (m_hasBeenFetch == false && cancellationToken.IsCancellationRequested == false)
            {
                try
                {
                    await InitializeRemoteConfigService();
                    await FetchRemoteConfigInternal();
                    await Task.Yield();
                }
                catch (Exception e)
                {
                    if (e is RequestFailedException requestFailedException)
                    {
                        Console.WriteLine(e);
                        HandleRequestFailedException();
                    }
                    else
                    {
                        Console.WriteLine(e);
                    }
                }
            }
        }

        public override void Delete()
        {
            if (RemoteConfigService.Instance != null)
            {
                RemoteConfigService.Instance.FetchCompleted -= OnFetchCompleted;
            }

            base.Delete();
        }
        #endregion LifeCycle

        #region Public Methods
        /// <summary>
        /// Fetch all the remote config allows to reset all the remote config value to the last version of the remote config.
        /// </summary>
        public async void FetchRemoteConfig()
        {
            await FetchRemoteConfigInternal();
        } 
        
        /// <summary>
        /// Register an event when the <see cref="remoteConfigValueBase"/> is update.
        /// </summary>
        public void RegisterOnValueFetched(RemoteConfigValueBase remoteConfigValueBase, RemoteConfigEvents.OnValueUpdated action)
        {
            m_remoteConfigEvents.RegisterDelegate(remoteConfigValueBase, action);
        }
        
        /// <summary>
        /// Unregister from <see cref="m_remoteConfigEvents"/>.
        /// </summary>
        public void UnregisterOnValueFetched(RemoteConfigValueBase remoteConfigValueBase, RemoteConfigEvents.OnValueUpdated action)
        {
            m_remoteConfigEvents.UnregisterDelegate(remoteConfigValueBase, action);
        }
        #endregion Public Methods

        #region Private Methods
        private async Task InitializeRemoteConfigService()
        {
            await UnityServices.InitializeAsync();

            if (AuthenticationService.Instance.IsSignedIn == false)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }
        }

        private async Task FetchRemoteConfigInternal()
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
                    QRLogger.DebugInfo<CoreTags.RemoteConfigs>($"No settings loaded this session, using default values.");
                    break;
                case ConfigOrigin.Cached:
                    QRLogger.DebugInfo<CoreTags.RemoteConfigs>($"No settings loaded this session, using cached values from a previous version.");
                    break;
                case ConfigOrigin.Remote:
                    QRLogger.DebugInfo<CoreTags.RemoteConfigs>($"New settings loaded this session, update values accordingly.");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            FetchAllRemoteConfigValues();
            QRLogger.DebugInfo<CoreTags.RemoteConfigs>($"Fetch completed : AppVersion = {m_remoteConfigSettings.AppVersion}.");
        }
        
        private void FetchAllRemoteConfigValues()
        {
            if (m_remoteConfigSettings == null)
            {
                m_remoteConfigSettings = RemoteConfigSettings.Instance;
            }
            
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
        
        private void RaiseOnValueChangeEvent(RemoteConfigValueBase remoteConfigValue)
        {
            m_remoteConfigEvents.RaiseOnValueChangedEvent(remoteConfigValue);
        }
        #endregion Private Methods

        #region Editor
        [Button("Fetch Remote Config")]
        private async void EditorFetchRemoteConfig()
        {
            await FetchRemoteConfigInternal();
        }
        #endregion Editor
        #endregion Methods
    }
}
