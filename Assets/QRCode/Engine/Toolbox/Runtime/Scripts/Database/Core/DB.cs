namespace QRCode.Engine.Toolbox.Database
{
    using System.Collections.Generic;
    using System.Linq;
    using QRCode.Engine.Debugging;
    using QRCode.Engine.Toolbox.Database.GeneratedEnums;
    using QRCode.Engine.Toolbox.Pattern.Singleton;
    using QRCode.Engine.Toolbox.Tags;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using Constants = QRCode.Engine.Toolbox.Constants;
    using TextGenerator = QRCode.Engine.Toolbox.TextGeneration.TextGenerator;

    /// <summary>
    /// This class should contains all the database of the game.
    /// </summary>
    [CreateAssetMenu(menuName = Constants.DatabasePath.BasePath + "DB", fileName = "DB")]
    public class DB : SingletonScriptableObject<DB>, IDatabase
    {
        #region Fields
        #region Serialized
        [TitleGroup(Constants.InspectorGroups.References)]
        [Tooltip("All the database of the project.")]
        [SerializeField] private Dictionary<string, ScriptableDatabaseBase> _allDatabase = null;
        
        [TitleGroup(Constants.InspectorGroups.Settings)]
        [Tooltip("The path where the enum will be generated or replaced.")]
        [FolderPath][ValidateInput("EditorCheckEnumPath", "Generated Enum Path should not be null.", InfoMessageType.Error)]
        [SerializeField] private string _generatedEnumPath = string.Empty;

        [TitleGroup(Constants.InspectorGroups.Settings)]
        [Tooltip("The namespace of the generated enum.")]
        [ValidateInput("EditorNamespace", "Namespace should not be null.", InfoMessageType.Error)]
        [SerializeField] private string _nameSpace = string.Empty;
        #endregion Serialized
        #endregion Fields

        #region Methods
        #region Public Methods
        /// <summary>
        /// Use this function to try to get a specific database from a generated key.
        /// /!\ a key must be generated from the database scriptable object.
        /// </summary>
        public T GetDatabase<T>(DBEnum key) where T : ScriptableDatabaseBase
        {
            if (TryGetDatabase<T>(key.ToString(), out var foundedDatabase))
            {
                return foundedDatabase as T;
            }

            return null;
        }
        #endregion Public Methods

        #region Private Methods
        private bool TryGetDatabase<T>(string key, out T foundedDatabase) where T : ScriptableDatabaseBase
        {
            if (_allDatabase == null)
            {
                foundedDatabase = null;
                QRLogger.DebugError<ToolboxTags.Databases>($"{_allDatabase} should not be null. Check if the database is well initialized.", this);
                return false;
            }
            
            if (_allDatabase.TryGetValue(key, out var outFoundedDatabase))
            {
                foundedDatabase = outFoundedDatabase as T;
                return true;
            }
            else
            {
                QRLogger.DebugError<ToolboxTags.Databases>($"Cannot find {key} in database.", this);
                foundedDatabase = null;
                return false;
            }
        }
        #endregion Private Methods

        #region Editor Methods
#if UNITY_EDITOR
        [Button("Generate Database Enum")]
        private void EditorGenerateDatabaseEnum()
        {
            if (string.IsNullOrEmpty(_generatedEnumPath))
            {
                QRLogger.DebugError<ToolboxTags.Databases>($"{nameof(_generatedEnumPath)} shouldn't be empty.");
                return;
            }
            
            var fields = new List<string>();

            for (int i = 0; i <_allDatabase.Count; i++)
            {
                fields.Add(_allDatabase.Keys.ElementAt(i));
            }
            
            TextGenerator.GenerateCSEnum(_generatedEnumPath, name + "Enum", _nameSpace, fields);
        }

        private bool EditorCheckEnumPath()
        {
            return !string.IsNullOrEmpty(_generatedEnumPath);
        }
        
        private bool EditorNamespace()
        {
            return !string.IsNullOrEmpty(_nameSpace);
        }
#endif
        #endregion Editor Methods
        #endregion Methods
    }
}
