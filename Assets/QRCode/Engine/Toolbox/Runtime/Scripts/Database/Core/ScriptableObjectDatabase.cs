namespace QRCode.Engine.Toolbox.Database
{
    using UnityEngine;

    using System.Collections.Generic;
    using System.Linq;
    
    using Sirenix.OdinInspector;

    using Debugging;
    using Toolbox;

    public abstract class ScriptableDatabaseBase : SerializedScriptableObject, IDatabase
    {
        
    }
    
    /// <summary>
    /// Inherit from this class to add a new database into the project.
    /// Don't forget to reference it into the database scriptable object.
    /// </summary>
    public abstract class ScriptableObjectDatabase<T> : ScriptableDatabaseBase
    {
        [TitleGroup(Constants.InspectorGroups.References)][InfoBox("@this.m_databaseInformation")]
        [SerializeField] protected Dictionary<string, T> m_database = new Dictionary<string, T>();
        [TitleGroup(Constants.InspectorGroups.Settings)]
        [SerializeField] protected string m_generatedEnumPath;

        protected abstract string m_databaseInformation
        {
            get;
        }
        
        /// <summary>
        /// Get the database dictionary.
        /// </summary>
        public Dictionary<string, T> GetDatabase
        {
            get
            {
                return m_database;
            }
        }
        
        /// <summary>
        /// Try to get an element from the database.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="foundedObject"></param>
        /// <returns></returns>
        public bool TryGetInDatabase(string key, out T foundedObject)
        {
            if (key == "Null")
            {
                foundedObject = default(T);
                return false;
            }
            
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
        
        [Button]
        private void GenerateDatabaseEnum()
        {
            if (string.IsNullOrEmpty(m_generatedEnumPath))
            {
                QRDebug.DebugError("Editor", $"{nameof(m_generatedEnumPath)} shouldn't be empty.");
                return;
            }
            
            QRDebug.Debug(Constants.DebuggingChannels.Database, $"Start generate database {name}...");
            
            var fields = new List<string> { "Null" };

            for (int i = 0; i < m_database.Count; i++)
            {
                fields.Add(m_database.Keys.ElementAt(i));
            }
            
            TextGeneration.TextGenerator.GenerateCSEnum(m_generatedEnumPath, name + "Enum", "QRCode.Framework", fields);
        }
    }
}
