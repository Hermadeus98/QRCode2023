namespace QRCode.Framework
{
    using System;
    using System.Collections.Generic;
    using Debugging;
    using Observer;
    using UnityEngine;
    using LogType = Debugging.LogType;

    public static class Observers
    {
        private static Dictionary<Type, Observer.Observer> m_allObservers = new Dictionary<Type, Observer.Observer>();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void Initialize()
        {
            Observer<EventArgs> m_floatObserver = new Observer<EventArgs>();
        }
        
        public static void RegisterObserver<T>(Observer<T> observer)
        {
            if (!m_allObservers.ContainsKey(typeof(T)))
            {
                m_allObservers.Add(typeof(T), observer as Observer<T>);
            }
        }

        public static void UnregisterObserver<T>(Observer<T> observer)
        {
            if (m_allObservers.ContainsKey(typeof(T)))
            {
                m_allObservers.Remove(typeof(T));
            }
        }

        public static Observer<T> GetObserver<T>()
        {
            if (!m_allObservers.ContainsKey(typeof(T)))
            {
                QRDebug.DebugMessage(LogType.Error, "Observers", $"Cannot find Observer of type {nameof(T)}");
            }
            
            return m_allObservers[typeof(T)] as Observer<T>;
        }
    }
}
