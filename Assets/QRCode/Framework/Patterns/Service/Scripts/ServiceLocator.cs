namespace QRCode.Framework
{
    using System;
    using System.Collections.Generic;
    using Debugging;

    public static class ServiceLocator
    {
        private static Dictionary<string, IService> m_services = new Dictionary<string, IService>();

        public static void Create()
        {
            m_services = new Dictionary<string, IService>();
        }

        public static void RegisterService<T>(IService service) where T : IService
        {
            var key = typeof(T).Name;
            
            if (!m_services.ContainsKey(key))
            {
                m_services.Add(key, service);
            }
        }

        public static T Get<T>() where T : IService
        {
            var key = typeof(T).Name;
            
            if (!m_services.ContainsKey(key))
            {
                QRDebug.DebugFatal( K.DebuggingChannels.Error,$"Cannot find service of type {key}. Add it into the ServiceLocator.");
                throw new InvalidOperationException();
            }

            return (T)m_services[key];
        }

        public static void InitializeService()
        {
            foreach (var service in m_services.Values)
            {
                service.OnInitialize();
            }
        }
    }
}
