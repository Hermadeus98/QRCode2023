namespace QRCode.Framework
{
    using System;
    using System.Linq;
    using Sirenix.OdinInspector;
    using UnityEngine;

    public class Settings<T> : SerializedScriptableObject, ISetting where T : ScriptableObject
    {
        private static T m_instance;

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
