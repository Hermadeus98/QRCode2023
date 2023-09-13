namespace QRCode.Engine.Core.RemoteConfig
{
    using System.Collections.Generic;
    using QRCode.Engine.Toolbox.Optimization;

    public class RemoteConfigEvents : IDeletable
    {
        #region Fields
        public delegate void OnValueUpdated();
        private Dictionary<RemoteConfigValueBase, OnValueUpdated> m_events = null;
        #endregion Fields

        #region Constructor
        public RemoteConfigEvents(RemoteConfigValueBase[] remoteConfigValues)
        {
            m_events = new Dictionary<RemoteConfigValueBase, OnValueUpdated>();
            InitEventsDictionary(remoteConfigValues);
        }
        #endregion Constructor

        #region Methods
        #region LifeCycle
        public void Delete()
        {
            if (m_events != null)
            {
                m_events.Clear();
                m_events = null;
            }
        }
        #endregion LifeCycle

        #region Public Methods
        public void RegisterDelegate(RemoteConfigValueBase key, OnValueUpdated action)
        {
            m_events[key] -= action;
            m_events[key] += action;
        }

        public void UnregisterDelegate(RemoteConfigValueBase key, OnValueUpdated action)
        {
            m_events[key] -= action;
        }

        public void RaiseOnValueChangedEvent(RemoteConfigValueBase key)
        {
            if (m_events[key] != null)
            {
                m_events[key].Invoke();
            }
        }
        #endregion Public Methods

        #region Private Methods
        private void InitEventsDictionary(RemoteConfigValueBase[] remoteConfigValues)
        {
            if (remoteConfigValues != null && remoteConfigValues.Length > 0)
            {
                foreach (var remoteConfigValue in remoteConfigValues)
                {
                    m_events.Add(remoteConfigValue, null);
                }
            }
        }
        #endregion Private Methods
        #endregion Methods
    }
}