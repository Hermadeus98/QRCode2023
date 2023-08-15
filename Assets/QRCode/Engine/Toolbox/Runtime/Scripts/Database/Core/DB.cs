namespace QRCode.Engine.Toolbox.Database
{
    using UnityEngine;

    using System.Collections.Generic;
    using System.Linq;
    
    using Sirenix.OdinInspector;

    using Debugging;
    using Toolbox;
    using GeneratedEnums;
    using Pattern.Singleton;
    using Constants = Toolbox.Constants;

    [CreateAssetMenu(menuName = Constants.DatabasePath.BasePath + "DB", fileName = "DB")]
    public class DB : SingletonScriptableObject<DB>, IDatabase
    {
        [TitleGroup(Constants.InspectorGroups.References)]
        [SerializeField] private Dictionary<string, ScriptableDatabaseBase> m_allDatabase = new Dictionary<string, ScriptableDatabaseBase>();
        
        [TitleGroup(Constants.InspectorGroups.Settings)]
        [SerializeField] private string m_generatedEnumPath;

        [TitleGroup(Constants.InspectorGroups.Settings)] 
        [SerializeField] private string m_nameSpace;

        /// <summary>
        /// Use this function to try to get a specific database from a generated key.
        /// /!\ a key must be generated from the database scriptable object.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="foundedDatabase"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool TryGetDatabase<T>(DBEnum key, out T foundedDatabase) where T : ScriptableDatabaseBase
        {
            return Instance.TryGetDatabase(key.ToString(), out foundedDatabase);
        }

        private bool TryGetDatabase<T>(string key, out T foundedDatabase) where T : ScriptableDatabaseBase
        {
            if (m_allDatabase.TryGetValue(key, out var outFoundedDatabase))
            {
                foundedDatabase = outFoundedDatabase as T;
                return true;
            }
            else
            {
                QRDebug.DebugError(Constants.DebuggingChannels.Database, $"Cannot find {key} in database.", this);
                foundedDatabase = null;
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

            for (int i = 0; i <m_allDatabase.Count; i++)
            {
                fields.Add(m_allDatabase.Keys.ElementAt(i));
            }
            
            Toolbox.TextGeneration.TextGenerator.GenerateCSEnum(m_generatedEnumPath, name + "Enum", m_nameSpace, fields);
        }
#endif
    }
}
