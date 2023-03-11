namespace QRCode.Framework
{
    using System;
    using System.Collections.Generic;
    using Debugging;
    using UnityEngine;

    public abstract class DatabaseBase
    {
        
    }
    
    [Serializable]
    public class Database<T> : DatabaseBase
    {
        [SerializeField] protected Dictionary<string, T> m_database = new Dictionary<string, T>();
        
        public Dictionary<string, T> GetDatabase
        {
            get
            {
                return m_database;
            }
        }
        
        public bool TryGetDatabase(string key, out T foundedObject)
        {
            if (m_database.TryGetValue(key, out foundedObject))
            {
                return true;
            }
            else
            {
                QRDebug.DebugError($"Scene Manager", $"Cannot find {key} in database.");
                return false;
            }
        }
    }

    [Serializable]
    public class Database<T, U> : DatabaseBase
    {
        [SerializeField] protected Dictionary<T, U> m_database = new Dictionary<T, U>();
        
        public Dictionary<T, U> GetDatabase
        {
            get
            {
                return m_database;
            }
        }
        
        public bool TryGetInDatabase(T key, out U foundedObject)
        {
            if (m_database.TryGetValue(key, out foundedObject))
            {
                return true;
            }
            else
            {
                QRDebug.DebugError($"Scene Manager", $"Cannot find {key} in database.");
                return false;
            }
        }

        public void AddToDatabase(T key, U element)
        {
            if (!m_database.ContainsKey(key))
            {
                m_database.Add(key, element);
            }
        }

        public void RemoveOfDatabase(T key)
        {
            if (m_database.ContainsKey(key))
            {
                m_database.Remove(key);
            }
        }
    }
}
