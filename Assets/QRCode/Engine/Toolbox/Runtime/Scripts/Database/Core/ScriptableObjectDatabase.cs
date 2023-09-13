namespace QRCode.Engine.Toolbox.Database
{
    using System.Collections.Generic;
    using System.Linq;
    using QRCode.Engine.Debugging;
    using QRCode.Engine.Toolbox.Tags;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using Constants = QRCode.Engine.Toolbox.Constants;
    
    public abstract class ScriptableDatabaseBase : SerializedScriptableObject, IDatabase { }
    
    /// <summary>
    /// Inherit from this class to add a new database into the project.
    /// Don't forget to reference it into <see cref="DB"/> scriptable object.
    /// </summary>
    public abstract class ScriptableObjectDatabase<t_parameter> : ScriptableDatabaseBase
    {
        #region Fields
        #region Serialized
        [TitleGroup(Constants.InspectorGroups.References)]
        [InfoBox("@this.m_databaseInformation")]
        [Tooltip("The database container.")]
        [SerializeField] protected Dictionary<string, t_parameter> m_database = new Dictionary<string, t_parameter>();
        
        [TitleGroup(Constants.InspectorGroups.Settings)]
        [Tooltip("The path where the enum will be generated or replaced.")]
        [FolderPath][ValidateInput("EditorCheckEnumPath", "Generated Enum Path should not be null.", InfoMessageType.Error)]
        [SerializeField] private string _generatedEnumPath = string.Empty;

        [TitleGroup(Constants.InspectorGroups.Settings)]
        [Tooltip("The namespace of the generated enum.")]
        [ValidateInput("EditorNamespace", "Namespace should not be null.", InfoMessageType.Error)]
        [SerializeField] private string _namespace = string.Empty;
        #endregion Serialized
        #endregion Fields

        #region Properties
        /// <summary>
        /// The database information panel shows in the inspector.
        /// </summary>
        protected abstract string m_databaseInformation
        {
            get;
        }

        /// <summary>
        /// Get the database dictionary.
        /// </summary>
        public Dictionary<string, t_parameter> GetDatabase { get { return m_database; } }
        #endregion Properties
        
        #region Methods
        /// <summary>
        /// Try to get an element from the database.
        /// </summary>
        public bool TryGetInDatabase(string key, out t_parameter foundedObject)
        {
            if (key == "Null")
            {
                foundedObject = default(t_parameter);
                return false;
            }
            
            if (m_database.TryGetValue(key, out foundedObject))
            {
                return true;
            }
            else
            {
                QRLogger.DebugError<ToolboxTags.Databases>($"Cannot find {key} in database.", this);
                return false;
            }
        }
        
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
            
            QRLogger.Debug<ToolboxTags.Databases>($"Start generate database {name}...");
            
            var fields = new List<string> { "Null" };

            for (int i = 0; i < m_database.Count; i++)
            {
                fields.Add(m_database.Keys.ElementAt(i));
            }
            
            TextGeneration.TextGenerator.GenerateCSEnum(_generatedEnumPath, name + "Enum", _namespace, fields);
        }
        
        private bool EditorCheckEnumPath()
        {
            return !string.IsNullOrEmpty(_generatedEnumPath);
        }
        
        private bool EditorNamespace()
        {
            return !string.IsNullOrEmpty(_namespace);
        }
#endif
        #endregion Editor Methods
        #endregion Methods
    }
}
