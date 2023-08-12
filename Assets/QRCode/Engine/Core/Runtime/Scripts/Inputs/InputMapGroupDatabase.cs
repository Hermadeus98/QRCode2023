namespace QRCode.Engine.Core.Inputs
{
    using UnityEngine;

    using System;
    using System.Collections.Generic;
    
    using Toolbox;
    using Toolbox.Database;

    [CreateAssetMenu(menuName = Constants.DatabasePath.Inputs + "Input Maps Groups Database", fileName = "DB_InputMapGroup")]
    public class InputMapGroupDatabase : ScriptableObjectDatabase<InputMapGroupData>
    {
        protected override string m_databaseInformation { get => "This database must contains all input map available in game."; }
    }

    [Serializable]
    public class InputMapGroupData
    {
        [SerializeField] private List<string> m_actionMaps = new List<string>();
        
        public List<string> ActionMaps => m_actionMaps;
    }
}
