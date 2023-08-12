namespace QRCode.Engine.Toolbox.Pattern.Singleton
{
    using System;
    using System.Linq;
    using Sirenix.OdinInspector;
    using UnityEngine;

    public class SingletonScriptableObject<T> : SerializedScriptableObject where T : ScriptableObject
    {
        private static T m_instance;

        public static T Instance
        {
            get
            {
                if (!m_instance)
                {
                    m_instance = Resources.LoadAll<T>("").FirstOrDefault();
                    (m_instance as SingletonScriptableObject<T>)?.OnInitialize();
                }
                if (!m_instance) throw new Exception($"Cannot find instance of {typeof(T)} in Resources.");
                if(m_instance) m_instance.hideFlags = HideFlags.DontUnloadUnusedAsset;
                return m_instance;
            }
        }

        // Optional overridable method for initializing the instance.
        protected virtual void OnInitialize() { }

    }
}
