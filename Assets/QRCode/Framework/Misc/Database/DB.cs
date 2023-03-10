namespace QRCode.Framework
{
    using System.Collections.Generic;
    using System.Linq;
    using Debugging;
    using Singleton;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using TextGenerator = Utils.TextGenerator;

    [CreateAssetMenu(menuName = K.DatabasePath.BasePath + "DB", fileName = "DB")]
    public class DB : SingletonScriptableObject<DB>
    {
        [SerializeField] private Dictionary<string, DatabaseBase> m_allDatabase = new Dictionary<string, DatabaseBase>();
        [SerializeField] private string m_generatedEnumPath;

        public static bool TryGetDatabase<T>(DBEnum key, out T foundedDatabase) where T : DatabaseBase
        {
            return Instance.TryGetDatabase(key.ToString(), out foundedDatabase);
        }
        
        public bool TryGetDatabase<T>(string key, out T foundedDatabase) where T : DatabaseBase
        {
            if (m_allDatabase.TryGetValue(key, out var outFoundedDatabase))
            {
                foundedDatabase = outFoundedDatabase as T;
                return true;
            }
            else
            {
                QRDebug.DebugError(K.DebugChannels.Database, $"Cannot find {key} in database.", this);
                foundedDatabase = null;
                return false;
            }
        }
        
#if UNITY_EDITOR
        [Button]
        private void GenerateDatabaseEnum()
        {
            /*for (int i = 0; i < m_allDatabase.Count; i++)
            {
                AssetDatabase.AddObjectToAsset(m_allDatabase.Values.ElementAt(i), this);
            }*/
            
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
            
            TextGenerator.GenerateCSEnum(m_generatedEnumPath, name + "Enum", "QRCode.Framework", fields);
        }
#endif
    }
}
