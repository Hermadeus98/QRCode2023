namespace QRCode.Engine.Toolbox.Settings
{
    using System;
    using System.Linq;
    using Sirenix.OdinInspector;
    using UnityEngine;

    /// <summary>
    /// All the settings of the application should be inherited of this class.
    /// </summary>
    public class Settings<T> : SerializedScriptableObject, ISetting where T : ScriptableObject
    {
        private static T m_instance;

        /// <summary>
        /// The instance of the setting.
        /// </summary>
        public static T Instance
        {
            get
            {
                if (!m_instance)
                {
                    m_instance = Resources.LoadAll<T>("").FirstOrDefault();
                }
                if (!m_instance) throw new Exception($"Cannot find instance of {typeof(T)} in Resources.");
                if(m_instance) m_instance.hideFlags = HideFlags.DontUnloadUnusedAsset;
                return m_instance;
            }
        }
    }
    
    public interface ISetting{}
}
