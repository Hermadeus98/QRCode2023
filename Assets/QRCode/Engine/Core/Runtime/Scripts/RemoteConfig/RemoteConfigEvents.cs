namespace QRCode.Engine.Core.RemoteConfig
{
    using System.Collections.Generic;

    public class RemoteConfigEvents
    {
        public delegate void OnValueUpdated();
        private Dictionary<RemoteConfigValueBase, OnValueUpdated> m_events = null;

        public RemoteConfigEvents(RemoteConfigValueBase[] remoteConfigValues)
        {
            m_events = new Dictionary<RemoteConfigValueBase, OnValueUpdated>();
            InitEventsDictionary(remoteConfigValues);
        }

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
    }
}