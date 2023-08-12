namespace QRCode.Engine.Toolbox.Database
{
    using System.Collections.Generic;
    using System.Linq;
    using Engine.Debugging;
    using Toolbox;
    using GeneratedEnums;
    using Sirenix.OdinInspector;
    using Pattern.Singleton;
    using UnityEngine;

    [CreateAssetMenu(menuName = Constants.DatabasePath.BasePath + "DB", fileName = "DB")]
    public class DB : SingletonScriptableObject<DB>, IDatabase
    {
        [TitleGroup(Constants.InspectorGroups.References)]
        [SerializeField] private Dictionary<string, ScriptableDatabaseBase> m_allDatabase = new Dictionary<string, ScriptableDatabaseBase>();
        
        [TitleGroup(Constants.InspectorGroups.Settings)]
        [SerializeField] private string m_generatedEnumPath;

        [TitleGroup(Constants.InspectorGroups.Settings)] 
        [SerializeField] private string m_nameSpace;

        public bool TryGetDatabase<T>(DBEnum key, out T foundedDatabase) where T : ScriptableDatabaseBase
        {
            return Instance.TryGetDatabase(key.ToString(), out foundedDatabase);
        }
        
        public bool TryGetDatabase<T>(string key, out T foundedDatabase) where T : ScriptableDatabaseBase
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
