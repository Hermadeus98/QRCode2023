namespace QRCode.Framework
{
    using System.Collections.Generic;
    using System.Linq;
    using Debugging;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using TextGenerator = Utils.TextGenerator;

    public abstract class DatabaseBase : SerializedScriptableObject{}
    
    public abstract class Database<T> : DatabaseBase
    {
        [SerializeField] protected Dictionary<string, T> m_database = new Dictionary<string, T>();
        [SerializeField] protected string m_generatedEnumPath;

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
                QRDebug.DebugError($"Scene Manager", $"Cannot find {key} in database.", this);
                return false;
            }
        }
        
#if UNITY_EDITOR
        [Button]
        private void GenerateDatabaseEnum()
        {
            if (string.IsNullOrEmpty(m_generatedEnumPath))
            {
                QRDebug.DebugError("Editor", $"{nameof(m_generatedEnumPath)} shouldn't be empty.");
                return;
            }
            
            var fields = new List<string>();

            for (int i = 0; i <m_database.Count; i++)
            {
                fields.Add(m_database.Keys.ElementAt(i));
            }
            
            TextGenerator.GenerateCSEnum(m_generatedEnumPath, name, "QRCode.Framework", fields);
        }
#endif
    }
}
