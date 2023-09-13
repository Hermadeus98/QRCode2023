namespace QRCode.Engine.Toolbox.Pattern.Service
{
    using System.Collections.Generic;
    using QRCode.Engine.Debugging;
    using QRCode.Engine.Toolbox.Tags;

    public abstract class ServiceLocatorBase{}
    
    /// <summary>
    /// This class should contain all the service of the application.
    /// A service is a manager that can be interchanged during the application execution.
    /// </summary>
    public abstract class ServiceLocatorBase<t_service> : ServiceLocatorBase where t_service : ServiceLocatorBase, new()
    {
        #region Fields
        /// <summary>
        /// The last created Service Locator.
        /// </summary>
        public static t_service Current = null;

        private readonly Dictionary<string, IService> m_services = null;
        #endregion Fields

        #region Methods
        /// <summary>
        /// Create a new <see cref="ServiceLocatorBase"/>.
        /// </summary>
        public static void Create()
        {
            Current = new t_service();
        }

        /// <summary>
        /// A service should be register before access to it.
        /// </summary>
        public void RegisterService<T>(IService service) where T : IService
        {
            var key = typeof(T).Name;
            
            if (!m_services.ContainsKey(key))
            {
                m_services.Add(key, service);
            }
        }

        /// <summary>
        /// Get a registered service from the appropriate service locator.
        /// </summary>
        public T GetService<T>() where T : IService
        {
            var key = typeof(T).Name;
            
            if (!m_services.ContainsKey(key))
            {
                QRLogger.DebugError<ToolboxTags.Services>($"Cannot find service of type {key}. Add it into the ServiceLocator.");
                return default;
            }

            return (T)m_services[key];
        }
        #endregion Methods
    }
}
