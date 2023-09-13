namespace QRCode.Engine.Toolbox.Database
{
    using System;
    using System.Collections.Generic;
    using Engine.Debugging;
    using QRCode.Engine.Toolbox.Optimization;
    using QRCode.Engine.Toolbox.Tags;
    using UnityEngine;

    public abstract class DatabaseBase : IDatabase, IDeletable
    {
        public virtual void Delete() { }
    }
    
    public interface IDatabase{}
    
    /// <summary>
    /// Create a database that will contains a value paired to a string key.
    /// </summary>
    [Serializable]
    public class Database<t_value> : DatabaseBase
    {
        #region Fields
        #region Serialized
        [Tooltip("Fill the database here, each key must be different.")]
        [SerializeField] protected Dictionary<string, t_value> _database = null;
        #endregion Serialized
        #endregion Fields

        #region Properties
        /// <summary>
        /// Get the database.
        /// </summary>
        public Dictionary<string, t_value> GetDatabase { get { return _database; } }
        #endregion Properties

        #region Constructor
        public Database(Dictionary<string, t_value> database)
        {
            _database = new Dictionary<string, t_value>(database);
        }
        #endregion Constructor
        
        #region Methods
        public override void Delete()
        {
            if (_database != null)
            {
                _database.Clear();
                _database = null;
            }
            
            base.Delete();
        }

        /// <summary>
        /// Try get an element into the database, will return an error if the element is not founded.
        /// </summary>
        public bool TryGetDatabase(string key, out t_value foundedObject)
        {
            if (_database.TryGetValue(key, out foundedObject))
            {
                return true;
            }
            else
            {
                QRLogger.DebugError<ToolboxTags.Databases>($"Cannot find {key} in database.");
                return false;
            }
        }
        #endregion Methods
    }

    /// <summary>
    /// Create a database that will contains a value paired to a key.
    /// </summary>
    [Serializable]
    public class Database<t_key, t_value> : DatabaseBase
    {
        #region Fields
        [Tooltip("Fill the database here, each key must be different.")]
        [SerializeField] protected Dictionary<t_key, t_value> _database = null;
        #endregion Fields

        #region Properties
        /// <summary>
        /// Get the database.
        /// </summary>
        public Dictionary<t_key, t_value> GetDatabase { get { return _database; } }
        #endregion Properties

        #region Constructor
        public Database(Dictionary<t_key, t_value> database)
        {
            _database = new Dictionary<t_key, t_value>(database);
        }
        #endregion Constructor
        
        #region Methods
        public override void Delete()
        {
            if (_database != null)
            {
                _database.Clear();
                _database = null;
            }
            
            base.Delete();
        }

        /// <summary>
        /// Try get an element into the database, will return an error if the element is not founded.
        /// </summary>
        public bool TryGetInDatabase(t_key key, out t_value foundedObject)
        {
            if (_database.TryGetValue(key, out foundedObject))
            {
                return true;
            }
            else
            {
                QRLogger.DebugError<ToolboxTags.Databases>($"Cannot find {key} in database.");
                return false;
            }
        }
        #endregion Methods
    }
}
