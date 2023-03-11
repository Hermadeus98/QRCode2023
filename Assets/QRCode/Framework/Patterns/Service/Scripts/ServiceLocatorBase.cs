namespace QRCode.Framework
{
    using System;
    using System.Collections.Generic;
    using Debugging;

    public abstract class ServiceLocatorBase{}
    
    public class ServiceLocatorBase<U> : ServiceLocatorBase where U : ServiceLocatorBase, new()
    {
        public static U Current = null;
        
        protected readonly Dictionary<string, IService> m_services = new Dictionary<string, IService>();

        public static void Create()
        {
            Current = new U();
        }

        public void RegisterService<T>(IService service) where T : IService
        {
            var key = typeof(T).Name;
            
            if (!m_services.ContainsKey(key))
            {
                m_services.Add(key, service);
            }
        }

        public T Get<T>() where T : IService
        {
            var key = typeof(T).Name;
            
            if (!m_services.ContainsKey(key))
            {
                QRDebug.DebugFatal( K.DebuggingChannels.Error,$"Cannot find service of type {key}. Add it into the ServiceLocator.");
                throw new InvalidOperationException();
            }

            return (T)m_services[key];
        }

        public void InitializeService()
        {
            foreach (var service in m_services.Values)
            {
                service.OnInitialize();
            }
        }
    }
}
